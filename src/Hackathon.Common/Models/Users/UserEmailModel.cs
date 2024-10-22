namespace Hackathon.Common.Models.Users;

/// <summary>
/// Email пользователя
/// </summary>
public class UserEmailModel
{
    /// <summary>
    /// Email пользователя
    /// </summary>
    public string Address { get; set; }

    /// <summary>
    /// Статус Email
    /// </summary>
    public UserEmailStatus Status { get; set; }

    /// <summary>
    /// Запрос подтверждения Email
    /// </summary>
    public EmailConfirmationRequestModel ConfirmationRequest { get; set; }
}
