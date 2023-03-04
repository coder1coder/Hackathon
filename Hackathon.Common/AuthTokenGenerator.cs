using Hackathon.Common.Configuration;
using Hackathon.Common.Models;
using Hackathon.Common.Models.User;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Hackathon.Common;

public static class AuthTokenGenerator
{
    /// <summary>
    /// Сгенерировать токен
    /// </summary>
    /// <param name="user">Пользователь</param>
    /// <param name="authOptions">Параметры для генерации токена</param>
    /// <returns></returns>
    public static AuthTokenModel GenerateToken(UserModel user, AuthOptions authOptions)
    {
        var expires = DateTimeOffset.UtcNow.AddMinutes(authOptions.LifeTime);

        var tokenHandler = new JwtSecurityTokenHandler();

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Role, ((int) user.Role).ToString())
        };

        if (user.GoogleAccount != null)
            claims.Add(new Claim(AppClaimTypes.GoogleId, user.GoogleAccount.Id));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = expires.UtcDateTime,
            Issuer = authOptions.Issuer,
            Audience = authOptions.Audience,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authOptions.Secret)),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        return new AuthTokenModel
        {
            UserId = user.Id,
            Expires = expires.ToUnixTimeMilliseconds(),
            Token = tokenString,
            Role = user.Role,
            GoogleId = user.GoogleAccount?.Id
        };
    }
}
