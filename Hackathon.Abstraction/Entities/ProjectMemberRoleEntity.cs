using Hackathon.DAL.Entities;

namespace Hackathon.Abstraction.Entities;

/// <summary>
/// Роль участника проекта
/// </summary>
public class ProjectMemberRoleEntity : BaseEntity
{
    /// <summary>
    /// Наименование роли участника проекта
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Участники проекта 
    /// </summary>
    public List<ProjectMemberEntity>? ProjectUsers { get; set; } = new ();
}