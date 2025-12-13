using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Web.Extensions;

public static class ConfigureAuthentication
{
    public static IServiceCollection AddJwtAuthentication(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var jwtSection = configuration.GetSection("Jwt");
        var secret = jwtSection["Secret"];
        var issuer = jwtSection["Issuer"];

        if (string.IsNullOrEmpty(secret))
            throw new InvalidOperationException("JWT Secret não configurado");

        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(
                JwtBearerDefaults.AuthenticationScheme,
                options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = issuer,
                        ValidAudience = issuer,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.ASCII.GetBytes(secret)
                        ),
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];

                            // Se a requisição é para o Hub do SignalR
                            var path = context.HttpContext.Request.Path;
                            if (
                                !string.IsNullOrEmpty(accessToken)
                                && path.StartsWithSegments("/chat")
                            )
                            {
                                context.Token = accessToken;
                            }

                            return Task.CompletedTask;
                        },
                    };
                }
            );

        services.AddAuthorization();

        return services;
    }
}
