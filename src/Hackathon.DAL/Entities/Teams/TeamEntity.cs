using System.Collections.Generic;
using Hackathon.Common.Models.Teams;
using Hackathon.DAL.Entities.Event;
using Hackathon.DAL.Entities.Interfaces;
using Hackathon.DAL.Entities.Users;

namespace Hackathon.DAL.Entities.Teams;

/// <summary>
/// Команда
/// </summary>
public class TeamEntity : BaseEntity, ISoftDeletable
{
    /// <summary>
    /// Наименование
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// События
    /// </summary>
    public ICollection<EventEntity> Events { get; set; } = new List<EventEntity>();

    /// <summary>
    /// Участники
    /// </summary>
    public ICollection<MemberTeamEntity> Members { get; set; } = new List<MemberTeamEntity>();

    /// <summary>
    /// Запросы на вступление в команду
    /// </summary>
    public ICollection<TeamJoinRequestEntity> JoinRequests { get; set; } = new List<TeamJoinRequestEntity>();

    /// <summary>
    /// Владелец команды
    /// </summary>
    public UserEntity Owner { get; set; }

    /// <summary>
    /// Идентификатор владельца команды
    /// </summary>
    public long? OwnerId { get; set; }

    /// <summary>
    /// Признак удаления
    /// </summary>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Тип команды
    /// </summary>
    public TeamType Type { get; set; }
}
