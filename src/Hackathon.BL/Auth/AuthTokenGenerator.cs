using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Hackathon.Common;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Auth;
using Hackathon.Configuration.Auth;
using Microsoft.IdentityModel.Tokens;

namespace Hackathon.BL.Auth;

public static class AuthTokenGenerator
{
    /// <summary>
    /// Сгенерировать токен
    /// </summary>
    /// <param name="payload">Данные для генерации токена авторизации</param>
    /// <param name="authenticateSettings">Настройки генерации токена</param>
    public static AuthTokenModel GenerateToken(GenerateTokenPayload payload, InternalAuthenticateSettings authenticateSettings)
    {
        var expires = DateTimeOffset.UtcNow.AddMinutes(authenticateSettings.LifeTime);

        var tokenHandler = new JwtSecurityTokenHandler();

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, payload.UserId.ToString()),
            new(ClaimTypes.Role, ((int) payload.UserRole).ToString())
        };

        if (payload.GoogleAccountId is not null)
        {
            claims.Add(new Claim(AppClaimTypes.GoogleId, payload.GoogleAccountId));
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = expires.UtcDateTime,
            Issuer = authenticateSettings.Issuer,
            Audience = authenticateSettings.Audience,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticateSettings.Secret)),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        return new AuthTokenModel
        {
            UserId = payload.UserId,
            Expires = expires.ToUnixTimeMilliseconds(),
            Token = tokenString,
            Role = payload.UserRole,
            GoogleId = payload.GoogleAccountId
        };
    }
}
