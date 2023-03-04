namespace Hackathon.Common.Models.Team;

/// <summary>
/// Модель фильтра списка команд
/// </summary>
public class TeamFilter
{
    /// <summary>
    /// Идентификаторы команд
    /// </summary>
    public long[] Ids { get; set; }

    /// <summary>
    /// Наименование команды
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Имя владельца
    /// </summary>
    public string Owner { get; set; }

    /// <summary>
    /// Количество участников в команде. От.
    /// </summary>
    public long? QuantityUsersFrom { get; set; }

    /// <summary>
    /// Количество участников в команде. До.
    /// </summary>
    public long? QuantityUsersTo { get; set; }

    /// <summary>
    /// Идентификатор события
    /// </summary>
    public long? EventId { get; set; }

    /// <summary>
    /// Идентификатор проекта
    /// </summary>
    public long? ProjectId { get; set; }

    /// <summary>
    /// Идентификатор владельца
    /// </summary>
    public long? OwnerId { get; set; }

    /// <summary>
    /// Есть владелец
    /// <remarks>создано реальным пользователем, не системой</remarks>
    /// </summary>
    public bool? HasOwner { get; set; }

    /// <summary>
    /// Идентификатор участника
    /// </summary>
    public long? MemberId { get; set; }
}
