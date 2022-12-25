namespace Hackathon.Common.Models.User;

/// <summary>
/// Статус Email пользователя
/// </summary>
public enum UserEmailStatus: byte
{
    /// <summary>
    /// Не подтвержден
    /// </summary>
    NotConfirmed = 0,

    /// <summary>
    /// Ожидает подтверждения
    /// </summary>
    Pending = 1,

    /// <summary>
    /// Подтвержден
    /// </summary>
    Confirmed = 2
}
