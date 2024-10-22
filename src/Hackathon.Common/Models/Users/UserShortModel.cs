using System;

namespace Hackathon.Common.Models.Users;

/// <summary>
/// Сокращенная модель пользователя
/// </summary>
public class UserShortModel
{
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Имя пользователя
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// Полное имя пользователя
    /// </summary>
    public string FullName { get; set; }

    /// <summary>
    /// Роль
    /// </summary>
    public UserRole Role { get; set; } = UserRole.Default;

    /// <summary>
    /// Идентификатор изображения профиля
    /// </summary>
    public Guid? ProfileImageId { get; set; }

    public string GetAnyName()
        => FullName ?? UserName;

    public override string ToString()
        => string.IsNullOrWhiteSpace(FullName)
            ? UserName
            : $"{FullName} ({UserName})";
}
