namespace Hackathon.Common.Models.User;

/// <summary>
/// Модель фильтра списка пользователей
/// </summary>
public class UserFilter
{
    /// <summary>
    /// Имя пользователя
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    /// Email
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Идентификаторы пользователей
    /// </summary>
    public long[] Ids { get; set; }

    /// <summary>
    /// Идентификаторы пользователей которые необходимо исключить из выборки
    /// </summary>
    public long[] ExcludeIds { get; set; }
}
