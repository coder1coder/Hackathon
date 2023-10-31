using System;
using Hackathon.Common.Models.User;

namespace Hackathon.Common.Models.Team;

public class TeamMember: UserShortModel
{
    /// <summary>
    /// Дата добавления пользователя в команду
    /// </summary>
    public DateTime DateTimeAdd { get; set; }

    /// <summary>
    /// Роль участника команды
    /// </summary>
    public TeamRole TeamRole { get; set; }
}
