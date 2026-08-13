using Microsoft.AspNetCore.Diagnostics;
using PestanaDevApi.AppConfig;
using PestanaDevApi.Interfaces.Repositories;
using PestanaDevApi.Interfaces.Services;
using PestanaDevApi.Repositories;
using PestanaDevApi.Services;
using PestanaDevApi.Utils;
using PestanaDevApi.Exceptions;
using PestanaDevApi.Services.Auth;
using PestanaDevApi.Interfaces.Services.Auth;
using PestanaDevApi.Services.Email;
using PestanaDevApi.Interfaces.Services.Email;
using PestanaDevApi.Interfaces.Factories;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using PestanaDevApi.Interfaces.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Threading.RateLimiting;
using PestanaDevApi.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// This allows Dapper to match any column containing an underscore. Therefore, 'user_name' will be processed as 'UserName'.
Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            return new BadRequestObjectResult(new
            {
                status = 400,
                message = "Validation error"
            });
        };
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add memory cache.
builder.Services.AddMemoryCache();

// Rate Limiting
builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
    {
        string? ip = context.Connection.RemoteIpAddress?.ToString();

        return RateLimitPartition.GetFixedWindowLimiter(ip!, _ =>
            new FixedWindowRateLimiterOptions
            {
                PermitLimit = 500,
                Window = TimeSpan.FromMinutes(30),
                QueueLimit = 0
            });
    });
});


// Setup secrets.
LocalSecretManagerConfig.Setup(builder.Environment.EnvironmentName, builder.Configuration);

// Setup Database connection
DbConfig.Setup(builder.Configuration, builder.Services);

// Setup Quartz
DbConfig.SetupQuartz(builder.Configuration, builder.Services);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = builder.Configuration["jwt.issuer"],

        IssuerSigningKey = new SymmetricSecurityKey(
            Convert.FromBase64String(builder.Configuration["jwt.key"]!)
        )
    };
});

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

#region Factories
builder.Services.AddSingleton<IDbConnectionFactory, MySqlConnectionFactory>();
#endregion

#region Services
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<ISignUpService, SignUpService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IEmailTemplateService, EmailTemplateService>();
builder.Services.AddScoped<IPlatformService, PlatformService>();
builder.Services.AddScoped<IPlatformAuthService, PlatformAuthService>();
builder.Services.AddScoped<IGoogleAuthService, GoogleAuthService>();
builder.Services.AddScoped<IGitHubAuthService, GitHubAuthService>();
builder.Services.AddScoped<ILinkedinAuthService, LinkedinAuthService>();
builder.Services.AddScoped<IConfirmationCodeService, ConfirmationCodeService>();
builder.Services.AddScoped<IForgotPasswordService, ForgotPasswordService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICaptchaService, CaptchaService>();
builder.Services.AddScoped<IRefreshTokenService, RefreshTokenService>();
builder.Services.AddScoped<INasaIntegrationService, NasaIntegrationService>();
builder.Services.AddScoped<INasaRequestService, NasaRequestService>();
builder.Services.AddScoped<INasaCacheService, NasaCacheService>();
builder.Services.AddScoped<ICacheService, CacheService>();
builder.Services.AddScoped<IMetropolitanMuseumRequestService, MetropolitanMuseumRequestService>();
builder.Services.AddScoped<IMetropolitanMuseumIntegrationService, MetropolitanMuseumIntegrationService>();


builder.Services.AddHttpClient<IRequestService, RequestService>((client =>
{
    client.BaseAddress = new Uri(builder.Configuration["api.baseUrl"]!);
    client.Timeout = TimeSpan.FromSeconds(30);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
}));
#endregion

#region Repositories
builder.Services.AddScoped<ISignUpRepository, SignUpRepository>();
builder.Services.AddScoped<IConfirmedEmailsRepository, ConfirmedEmailsRepository>();
builder.Services.AddScoped<ILoginRepository, LoginRepository>();
builder.Services.AddScoped<ITokenRepository, TokenRepository>();
builder.Services.AddScoped<IConfirmationCodeGenerationRepository, ConfirmationCodeGenerationRepository>();
builder.Services.AddScoped<IForgotPasswordRepository, ForgotPasswordRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

#endregion

#region Utils
builder.Services.AddScoped<IGoogleApisHttpClient, GoogleApisHttpClient>();
builder.Services.AddHttpClient<GoogleApisHttpClient>();
#endregion

var app = builder.Build();

app.UseExceptionHandler(builder =>
{
    builder.Run(async context =>
    {
        ILogger logger = context.RequestServices.GetRequiredService<ILogger<Program>>();

        Exception? error = context.Features.Get<IExceptionHandlerFeature>()?.Error;
        int statusCode = error switch
        {
            ApiException apiEx => apiEx.StatusCode,
            UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
            KeyNotFoundException => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status500InternalServerError
        };

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        logger.LogError(error, "Unhandled exception");

        var response = new
        {
            status = statusCode,
            message = GetHttpMessage.Get(statusCode),
        };

        await context.Response.WriteAsJsonAsync(response);
    });
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseRateLimiter();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
