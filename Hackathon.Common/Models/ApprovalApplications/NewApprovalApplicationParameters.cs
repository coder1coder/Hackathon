using System;

namespace Hackathon.Common.Models.ApprovalApplications;

public class NewApprovalApplicationParameters
{
    /// <summary>
    /// Идентификатор мероприятия
    /// </summary>
    public long EventId { get; set; }

    /// <summary>
    /// Идентификатор автора заявки
    /// </summary>
    public long AuthorId { get; set; }

    /// <summary>
    /// Статус заявки
    /// </summary>
    public ApprovalApplicationStatus ApplicationStatus { get; set;}

    /// <summary>
    /// Дата и время создания заявки
    /// </summary>
    public DateTimeOffset RequestedAt { get; set; }
}
