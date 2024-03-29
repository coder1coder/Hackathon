namespace Hackathon.Common.Models.Team;

public class CreateTeamModel
{
    public string Name { get; set; }
    public long? EventId { get; set; }
    public long? OwnerId { get; set; }

    /// <summary>
    /// Тип команды
    /// </summary>
    public TeamType Type { get; set; }
}