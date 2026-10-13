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
using Microsoft.AspNetCore.HttpOverrides;
using PestanaDevApi.Constants;
using PestanaDevApi.Hubs;

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

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor |
        ForwardedHeaders.XForwardedProto;
});

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

// Setup JWT authentication (main token + hub token schemes)
JwtConfig.Setup(builder.Configuration, builder.Services);

builder.Services.AddSignalR();

// The SignalR JS client sends credentials on negotiate, so origins must be explicit (no AllowAnyOrigin).
string[] allowedOrigins = builder.Configuration.GetSection(CorsConstants.AllowedOriginsConfigKey).Get<string[]>() ?? [];

builder.Services.AddCors(options =>
{
    options.AddPolicy(CorsConstants.FrontEndPolicy, policy =>
    {
        policy.WithOrigins(allowedOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
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
builder.Services.AddSingleton<ITranslationService, TranslationService>();
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
builder.Services.AddScoped<IMetropolitanMuseumCacheService, MetropolitanMuseumCacheService>();
builder.Services.AddScoped<ISemaphoreService, SemaphoreService>();
builder.Services.AddKeyedSingleton<ISemaphoreService>(SemaphoreConstants.BoardSemaphoreKey, (_, _) => new SemaphoreService());
builder.Services.AddScoped<IBoardService, BoardService>();
builder.Services.AddScoped<IBoardPanelService, BoardPanelService>();
builder.Services.AddScoped<IBoardTaskService, BoardTaskService>();
builder.Services.AddScoped<IBoardColumnService, BoardColumnService>();
builder.Services.AddScoped<IBoardMemberService, BoardMemberService>();
builder.Services.AddScoped<IBoardCacheService, BoardCacheService>();
builder.Services.AddScoped<IBoardNotificationService, BoardNotificationService>();
builder.Services.AddSingleton<IBoardHubConnectionService, BoardHubConnectionService>();


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
builder.Services.AddScoped<IBoardRepository, BoardRepository>();
builder.Services.AddScoped<IBoardPanelRepository, BoardPanelRepository>();
builder.Services.AddScoped<IBoardTaskRepository, BoardTaskRepository>();
builder.Services.AddScoped<IBoardColumnRepository, BoardColumnRepository>();
builder.Services.AddScoped<IBoardMemberRepository, BoardMemberRepository>();

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

app.UseRateLimiter();

app.UseForwardedHeaders();


app.UseHttpsRedirection();

app.UseRouting();

app.UseCors(CorsConstants.FrontEndPolicy);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
// An already open connection is kept when its hub token expires: the token is only checked when connecting.
app.MapHub<BoardHub>(SignalRConstants.BoardHubPath, options => options.CloseOnAuthenticationExpiration = false);

app.Run();
