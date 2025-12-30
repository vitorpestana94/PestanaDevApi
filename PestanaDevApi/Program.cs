using Microsoft.AspNetCore.Diagnostics;
using PestanaDevApi.AppConfig;
using PestanaDevApi.Interfaces.Repositories;
using PestanaDevApi.Interfaces.Services;
using PestanaDevApi.Repositories;
using PestanaDevApi.Services;
using PestanaDevApi.Utils;

var builder = WebApplication.CreateBuilder(args);

// This allows Dapper to match any column containing an underscore. Therefore, 'user_name' will be processed as 'UserName'.
Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Setup secrets.
LocalSecretManagerConfig.Setup(builder.Environment.EnvironmentName, builder.Configuration);

// Setup Database connection
DbConfig.Setup(builder.Configuration, builder.Services);

#region Services

builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<ISignUpService, SignUpService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IPlatformAuthService, PlatformAuthService>();
#endregion

#region Repositories

builder.Services.AddScoped<ISignUpRepository, SignUpRepository>();
builder.Services.AddScoped<ILoginRepository, LoginRepository>();
builder.Services.AddScoped<ITokenRepository, TokenRepository>();

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler(builder =>
{
    builder.Run(async context =>
    {
        context.Response.ContentType = "application/json";

        Exception? error = context.Features.Get<IExceptionHandlerFeature>()?.Error;

        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

        await context.Response.WriteAsJsonAsync(new
        {
            message = ApiLib.GetErrorMessage(context.Response.StatusCode, error)
        });
    });
});


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
