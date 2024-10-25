namespace Hackathon.Common.Messages.Events;

/// <summary>
/// Сообщение о созданном мероприятии
/// </summary>
/// <param name="EventId">Идентификатор мероприятия</param>
/// <param name="OwnerId">Идентификатор пользователя</param>
public record EventCreatedMessage(long EventId, long OwnerId)
{
    /// <summary>
    /// Идентификатор мероприятия
    /// </summary>
    public long EventId { get; } = EventId;
    
    /// <summary>
    /// Автор мероприятия
    /// </summary>
    public long OwnerId { get; } = OwnerId;
}
