using System.ComponentModel.DataAnnotations;

namespace Hackathon.API.Contracts.Projects;

/// <summary>
/// Запрос на создание проекта
/// </summary>
public class ProjectCreateRequest
{
    /// <summary>
    /// Наименование проекта
    /// </summary>
    [Required(AllowEmptyStrings = false)] 
    [StringLength(100, MinimumLength = 3)]
    public string Name { get; set; }

    /// <summary>
    /// Описание проекта
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Идентификатор команды
    /// </summary>
    [Required]
    public long TeamId { get; set; }

    /// <summary>
    /// Идентификатор мероприятия
    /// </summary>
    [Required]
    public long EventId { get; set; }
}
