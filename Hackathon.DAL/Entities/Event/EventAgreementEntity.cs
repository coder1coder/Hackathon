using System.Collections.Generic;
using Hackathon.DAL.Entities.User;

namespace Hackathon.DAL.Entities.Event;

/// <summary>
/// Соглашение участия в мероприятии
/// </summary>
public class EventAgreementEntity
{
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
    public long EventId { get; set; }
    public EventEntity Event { get; set; }

    /// <summary>
    /// Пользователи, подтвердившие соглашение
    /// </summary>
    public ICollection<UserEntity> Users { get; set; } = new List<UserEntity>();
}
