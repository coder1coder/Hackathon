using Hackathon.Entities.User;

namespace Hackathon.Entities;

/// <summary>
/// Запрос на подтверждение Email пользователя
/// </summary>
public class EmailConfirmationRequestEntity
{
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    /// Пользователь
    /// </summary>
    public UserEntity User { get; set; }

    /// <summary>
    /// Email пользователя
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Признак подтверждения запроса
    /// </summary>
    public bool IsConfirmed { get; set; }

    /// <summary>
    /// Код подтверждения
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Дата и время создания запроса в UTC
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Дата подтверждения Email в UTC
    /// </summary>
    public DateTime? ConfirmationDate { get; set; }
}
