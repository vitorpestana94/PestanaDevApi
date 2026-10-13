using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using PestanaDevApi.Constants;

namespace PestanaDevApi.AppConfig
{
    public class JwtConfig
    {
        /// <summary>
        /// Sets up the two authentication schemes of the API:
        /// - the default one (main JWT), used by every controller, which rejects hub tokens by their audience;
        /// - the "HubToken" one, used only by the SignalR hubs, which only accepts hub tokens and reads them
        ///   from the "access_token" query string on /hubs requests (browsers can't send headers on WebSockets).
        /// </summary>
        /// <param name="configuration">
        /// The app configuration, containing "jwt.issuer" and "jwt.key".
        /// </param>
        /// <param name="services">
        /// The app service collection.
        /// </param>
        public static void Setup(IConfiguration configuration, IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = GetTokenValidationParameters(configuration);
                options.TokenValidationParameters.AudienceValidator = (audiences, _, _) => IsApiAudience(audiences);
            })
            .AddJwtBearer(TokenConstants.HubTokenScheme, options =>
            {
                options.TokenValidationParameters = GetTokenValidationParameters(configuration);
                options.TokenValidationParameters.ValidAudience = TokenConstants.HubAudience;

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        string? accessToken = context.Request.Query[SignalRConstants.AccessTokenQueryKey];

                        if (!string.IsNullOrEmpty(accessToken) && context.HttpContext.Request.Path.StartsWithSegments(SignalRConstants.HubsBasePath))
                            context.Token = accessToken;

                        return Task.CompletedTask;
                    }
                };
            });
        }

        /// <summary>
        /// Validates the audience of the main JWT. Hub tokens (and any other audience) are rejected.
        /// Tokens without audience are still accepted because the main tokens issued before the hub token
        /// existed don't have one; this can be removed once they expire (60 minutes after the deploy).
        /// </summary>
        /// <param name="audiences">
        /// The token audiences.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the token can access the API endpoints; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool IsApiAudience(IEnumerable<string>? audiences)
        {
            if (audiences == null || !audiences.Any())
                return true;

            return audiences.All(audience => audience == TokenConstants.ApiAudience);
        }

        #region Private Methods
        private static TokenValidationParameters GetTokenValidationParameters(IConfiguration configuration)
        {
            return new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = configuration["jwt.issuer"],

                IssuerSigningKey = new SymmetricSecurityKey(
                    Convert.FromBase64String(configuration["jwt.key"]!)
                )
            };
        }
        #endregion
    }
}
