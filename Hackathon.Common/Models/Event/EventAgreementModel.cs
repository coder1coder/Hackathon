using System.Collections.Generic;
using Hackathon.Common.Models.User;

namespace Hackathon.Common.Models.Event;

/// <summary>
/// Соглашение об участии в мероприятии
/// </summary>
public class EventAgreementModel
{
    /// <summary>
    /// Идентификатор мероприятия
    /// </summary>
    public long EventId { get; set; }

    /// <summary>
    /// Правила проведения мероприятия
    /// </summary>
    public string Rules { get; set; }

    /// <summary>
    /// Требуется ли подтверждение соглашения
    /// </summary>
    public bool RequiresConfirmation { get; set; }

    /// <summary>
    /// Мероприятие
    /// </summary>
    public EventModel Event { get; set; }

    /// <summary>
    /// Пользователи принявшие соглашение об участии в мероприятии
    /// </summary>
    public List<UserShortModel> Users { get; set; } = new();
}
