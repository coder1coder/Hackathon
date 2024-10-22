using Hackathon.Common.Models.Users;

namespace Hackathon.API.Contracts.Users;

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
