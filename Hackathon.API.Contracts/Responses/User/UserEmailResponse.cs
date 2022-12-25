using Hackathon.Common.Models.User;

namespace Hackathon.Contracts.Responses.User;

public class UserEmailResponse
{
    /// <summary>
    /// Email пользователя
    /// </summary>
    public string Address { get; set; }

    /// <summary>
    /// Статус Email
    /// </summary>
    public UserEmailStatus Status { get; set; }
}
