using System;
using Hackathon.Common.Models.Users;

namespace Hackathon.Common.Models.Teams;

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
