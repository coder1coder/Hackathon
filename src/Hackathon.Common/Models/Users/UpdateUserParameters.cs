namespace Hackathon.Common.Models.Users;

public class UpdateUserParameters
{
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Полное имя пользователя
    /// </summary>
    public string FullName { get; set; }

    /// <summary>
    /// Email пользователя
    /// </summary>
    public string Email { get; set; }
}
