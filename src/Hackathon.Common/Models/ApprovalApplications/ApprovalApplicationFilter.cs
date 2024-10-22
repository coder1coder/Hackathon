namespace Hackathon.Common.Models.ApprovalApplications;

/// <summary>
/// Фильтр заявки на модерацию
/// </summary>
public class ApprovalApplicationFilter
{
    /// <summary>
    /// Идентификатор мероприятия
    /// </summary>
    public long? EventId { get; set; }

    /// <summary>
    /// Статус
    /// </summary>
    public ApprovalApplicationStatus? Status { get; set; }
}
