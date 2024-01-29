namespace Hackathon.Common.Messages;

/// <summary>
/// Сообщение о созданном мероприятии
/// </summary>
/// <param name="EventId">Идентификатор мероприятия</param>
/// <param name="UserId">Идентификатор пользователя</param>
public record EventCreatedMessage(
    long EventId,
    long UserId);
