namespace Hackathon.Common.Models.Team;

public class ChangeOwnerModel
{
    /// <summary>
    /// Идентификатор команды
    /// </summary>
    public long TeamId { get; set; }

    /// <summary>
    /// Владелец команды
    /// </summary>
    public long OwnerId { get; set; }

    /// <summary>
    /// Новый владелец команды
    /// </summary>
    public long NewOwnerId { get; set; }
}