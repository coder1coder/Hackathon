using System;
using Hackathon.Common.Models.ApprovalApplications;
using Hackathon.Common.Models.User;

namespace Hackathon.Common.Models.Event;

public class ApprovalApplicationModel
{
    /// <summary>
    /// Идентификатор заявки
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Статус согласования
    /// </summary>
    public ApprovalApplicationStatus ApplicationStatus { get; set; }

    /// <summary>
    /// Подписант
    /// </summary>
    public long SignerId { get; set; }

    /// <summary>
    /// Идентификатор автора запроса
    /// </summary>
    public long AuthorId { get; set; }

    /// <summary>
    /// Автор запроса
    /// </summary>
    public UserModel Author { get; set; }

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
}
