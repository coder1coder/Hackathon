using System.ComponentModel.DataAnnotations;

namespace Hackathon.Contracts.Requests.ProjectMember;

/// <summary>
/// Запрос на создание нового участника проекта
/// </summary>
public class CreateProjectMemberRequest
{
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    [Required]
    public long UserId { get; set; }
    
    /// <summary>
    /// Идентификатор проекта
    /// </summary>
    [Required]
    public long ProjectId { get; set; }
    
    /// <summary>
    /// Идентификатор команды
    /// </summary>
    [Required]
    public long TeamId { get; set; }
    
    /// <summary>
    /// Идентификатор события
    /// </summary>
    public long? EventId { get; set; }
}