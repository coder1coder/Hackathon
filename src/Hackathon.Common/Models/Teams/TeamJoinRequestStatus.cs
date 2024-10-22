namespace Hackathon.Common.Models.Teams;

/// <summary>
/// Статус запроса на вступление в команду
/// </summary>
public enum TeamJoinRequestStatus: byte
{
    /// <summary>
    /// Запрос отправлен
    /// </summary>
    Sent = 0,

    /// <summary>
    /// Запрос принят
    /// </summary>
    Accepted = 1,

    /// <summary>
    /// Запрос отклонен
    /// </summary>
    Refused = 2,

    /// <summary>
    /// Запрос отменен автором
    /// </summary>
    Cancelled = 3
}
