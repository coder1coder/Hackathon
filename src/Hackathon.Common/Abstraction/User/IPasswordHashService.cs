﻿using System.Threading.Tasks;

namespace Hackathon.Common.Abstraction.User;

/// <summary>
/// Сервис для работы с хешем пароля
/// </summary>
public interface IPasswordHashService
{
    /// <summary>
    /// Проверить пароль и хеш
    /// </summary>
    /// <param name="password">Пароль</param>
    /// <param name="passwordHash">Хеш</param>
    Task<bool> VerifyAsync(string password, string passwordHash);
    
    /// <summary>
    /// Захешировать пароль
    /// </summary>
    /// <param name="password">Пароль</param>
    string HashPassword(string password);
}
