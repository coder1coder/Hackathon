using Hackathon.Common.Models;
using Hackathon.Common.Models.User;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Hackathon.Configuration;

namespace Hackathon.Common;

public static class AuthTokenGenerator
{
    /// <summary>
    /// Сгенерировать токен
    /// </summary>
    /// <param name="userSignInDetails">Пользователь</param>
    /// <param name="authOptions">Параметры для генерации токена</param>
    /// <returns></returns>
    public static AuthTokenModel GenerateToken(UserSignInDetails userSignInDetails, AuthOptions authOptions)
    {
        var expires = DateTimeOffset.UtcNow.AddMinutes(authOptions.LifeTime);

        var tokenHandler = new JwtSecurityTokenHandler();

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userSignInDetails.UserId.ToString()),
            new(ClaimTypes.Role, ((int) userSignInDetails.UserRole).ToString())
        };

        if (userSignInDetails.GoogleAccountId is not null)
            claims.Add(new Claim(AppClaimTypes.GoogleId, userSignInDetails.GoogleAccountId));

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
            UserId = userSignInDetails.UserId,
            Expires = expires.ToUnixTimeMilliseconds(),
            Token = tokenString,
            Role = userSignInDetails.UserRole,
            GoogleId = userSignInDetails.GoogleAccountId
        };
    }
}
