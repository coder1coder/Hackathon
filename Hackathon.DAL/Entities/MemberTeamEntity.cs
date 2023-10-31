using Hackathon.Common.Models.Team;
using Hackathon.DAL.Entities.User;
using System;

namespace Hackathon.DAL.Entities;

/// <summary>
/// Таблица для связи участников и команд (отношение многие ко многим)
/// </summary>
public class MemberTeamEntity
{
    /// <summary>
    /// Идентификатор команды
    /// </summary>
    public long TeamId { get; set; }

    /// <summary>
    /// Команда
    /// </summary>
    public TeamEntity Team { get; set; } = new();

    /// <summary>
    /// Идентификатор участника команды
    /// </summary>
    public long MemberId { get; set; }

    /// <summary>
    /// Участник команды
    /// </summary>
    public UserEntity Member { get; set; } = new();

    /// <summary>
    /// Дата/время добавления участника в команду (UTC)
    /// </summary>
    public DateTime DateTimeAdd { get; set; }

    /// <summary>
    /// Роль участника команды
    /// </summary>
    public TeamRole Role { get; set; }
}
