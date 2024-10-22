namespace Hackathon.Common.Models.Users;

/// <summary>
/// Параметры запроса на подтверждение Email
/// </summary>
public class EmailConfirmationRequestParameters
{
    /// <summary>
    /// Email пользователя
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    /// Код подтверждения
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Признак подтверждения Email
    /// </summary>
    public bool IsConfirmed { get; set; }
}
