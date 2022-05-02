using Hackathon.DAL.Entities;

namespace Hackathon.Abstraction.Entities;

public class TeamEventEntity: BaseEntity
{
    /// <summary>
    /// Идентификатор команды
    /// </summary>
    public long TeamId { get; set; }
    
    public TeamEntity? Team { get; set; }

    /// <summary>
    /// Идентификатор события
    /// </summary>
    public long EventId { get; set; }
    public EventEntity? Event { get; set; }

    /// <summary>
    /// Идентификатор проекта
    /// </summary>
    public long? ProjectId { get; set; }
    public ProjectEntity? Project { get; set; }

    
}