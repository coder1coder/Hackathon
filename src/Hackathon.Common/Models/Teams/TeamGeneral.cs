namespace Hackathon.Common.Models.Teams;

/// <summary>
/// Модель общей информации команды
/// </summary>
public class TeamGeneral
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Наименование
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Владелец
    /// </summary>
    public TeamMember Owner { get; set; }

    /// <summary>
    /// Участники команды
    /// </summary>
    public TeamMember[] Members { get; set; }
}
