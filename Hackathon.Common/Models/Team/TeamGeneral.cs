using Hackathon.Common.Models.User;

namespace Hackathon.Common.Models.Team;

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
    public UserModel Owner { get; set; }
    
    /// <summary>
    /// Участники команды
    /// </summary>
    public UserModel[] Members { get; set; }
}
