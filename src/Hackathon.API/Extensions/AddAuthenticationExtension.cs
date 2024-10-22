using System;
using System.Text;
using Hackathon.Configuration.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Hackathon.API.Extensions;

internal static class AddAuthenticationExtension
{
    public static IServiceCollection AddAuthentication(this IServiceCollection services, AuthenticateSettings authenticateSettings)
    {
        if (authenticateSettings is null)
        {
            throw new ArgumentNullException(nameof(authenticateSettings), "Необходимо указать параметры аутентификации/авторизации");
        }

        services
            .AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticateSettings.Internal.Secret)),
                    ValidIssuer = authenticateSettings.Internal.Issuer,
                    ValidAudience = authenticateSettings.Internal.Audience
                };
            });

        return services;
    }
}
