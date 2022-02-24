using Hackathon.DAL.Entities;

namespace Hackathon.Abstraction.Entities;

public class TeamEventEntity: BaseEntity
{
    public long TeamId { get; set; }
    public TeamEntity? Team { get; set; }

    public long EventId { get; set; }
    public EventEntity? Event { get; set; }

    public long? ProjectId { get; set; }
    public ProjectEntity? Project { get; set; }

    
}