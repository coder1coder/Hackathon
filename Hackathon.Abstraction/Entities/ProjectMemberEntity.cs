using Hackathon.DAL.Entities;

namespace Hackathon.Abstraction.Entities;

/// <summary>
/// Участник проекта
/// </summary>
public class ProjectMemberEntity : BaseEntity
{
    public UserEntity? User { get; set; }
    public long? UserId { get; set; }

    public long? TeamId { get; set; }
    public TeamEntity? Team { get; set; }

    public long? ProjectId { get; set; }
    public ProjectEntity? Project { get; set; }

    public long? EventId { get; set; }
    public EventEntity? Event { get; set; }

    /// <summary>
    /// Роли участника проекта
    /// </summary>
    public List<string>? ProjectMemberRoles { get; set; } = new();
}