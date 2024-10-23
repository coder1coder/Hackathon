namespace Hackathon.Common.Models.Teams;

/// <summary>
/// Роль участника команды
/// </summary>
public enum TeamRole : byte
{
    /// <summary>
    /// Участник
    /// </summary>
    Participant = 0,
    
    /// <summary>
    /// Владелец
    /// </summary>
    Owner = 1
}
