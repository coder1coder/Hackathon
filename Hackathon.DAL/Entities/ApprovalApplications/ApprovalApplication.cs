using System;
using Hackathon.Common.Models.ApprovalApplications;
using Hackathon.DAL.Entities.Event;
using Hackathon.DAL.Entities.User;

namespace Hackathon.DAL.Entities.ApprovalApplications;

/// <summary>
/// Заявка на согласование
/// </summary>
public class ApprovalApplication: BaseEntity
{
    /// <summary>
    /// Статус согласования
    /// </summary>
    public ApprovalApplicationStatus ApplicationStatus { get; set; }

    /// <summary>
    /// Подписант
    /// </summary>
    public long SignerId { get; set; }

    /// <summary>
    /// Автор запроса
    /// </summary>
    public long AuthorId { get; set; }

    public UserEntity Author { get; set; }

    /// <summary>
    /// Комментарий
    /// </summary>
    public string Comment { get; set; }

    /// <summary>
    /// Дата и время заявки
    /// </summary>
    public DateTimeOffset RequestedAt { get; set; }

    /// <summary>
    /// Дата и время решения по заявке
    /// </summary>
    public DateTimeOffset? DecisionAt { get; set; }

    /// <summary>
    /// Мероприятие
    /// </summary>
    public EventEntity Event { get; set; }
}
