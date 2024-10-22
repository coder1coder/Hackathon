using System.ComponentModel.DataAnnotations;
using Hackathon.Common.Models.Teams;

namespace Hackathon.API.Contracts.Teams;

/// <summary>
/// Запрос на создание новой команды
/// </summary>
public class CreateTeamRequest
{
    /// <summary>
    /// Наименование команды
    /// </summary>
    [Required]
    public string Name { get; set; }

    /// <summary>
    /// Идентификатор события
    /// </summary>
    public long? EventId { get; set; }

    /// <summary>
    /// Тип команды
    /// </summary>
    public TeamType Type { get; set; } = TeamType.Private;
}
