namespace Hackathon.Contracts.Requests.Event;

/// <summary>
/// Контракт создания и обновления нового события
/// </summary>
public class UpdateEventRequest: CreateEventRequest
{
    /// <summary>
    /// Идентификатор обновляемой записи
    /// </summary>
    public long Id { get; set; }
}