using System;
using Hackathon.Common.Models.User;
using System.Linq;

namespace Hackathon.Common.Models.Team;

public class TeamModel
{
    public long Id { get; set; }
    public string Name { get; set; }
    public UserModel[] Members { get; set; } = Array.Empty<UserModel>();
    public UserModel Owner { get; set; }
    public long? OwnerId { get; set; }

    public TeamType Type { get; set; }

    /// <summary>
    /// Проверить, является ли пользователь участником команды
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public bool HasMember(long userId)
        => Members.FirstOrDefault(x => x.Id == userId) is not null || HasOwner(userId);

    /// <summary>
    /// Проверить, является ли пользователь владельцем команды
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public bool HasOwner(long userId)
        => OwnerId == userId;
}
