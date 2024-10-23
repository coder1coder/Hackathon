using System;
using System.Linq;

namespace Hackathon.Common.Models.Teams;

public class TeamModel
{
    /// <summary>
    /// Максимальное количество участников в команде
    /// </summary>
    public const int MembersLimit = 30;

    public long Id { get; set; }
    public string Name { get; set; }
    public TeamMember[] Members { get; set; } = Array.Empty<TeamMember>();
    public TeamMember Owner { get; set; }
    public long? OwnerId { get; set; }

    public TeamType Type { get; set; }

    /// <summary>
    /// Проверить, является ли пользователь участником команды
    /// </summary>
    /// <param name="userId"></param>
    public bool HasMemberWithId(long userId)
        => Members.FirstOrDefault(x => x.Id == userId) is not null || HasOwnerWithId(userId);

    /// <summary>
    /// Проверить, является ли пользователь владельцем команды
    /// </summary>
    /// <param name="userId"></param>
    public bool HasOwnerWithId(long userId)
        => OwnerId == userId;

    /// <summary>
    /// Проверить, является ли команда заполненной
    /// </summary>
    public bool IsFull()
        => Members.Length + 1 >= MembersLimit;

    /// <summary>
    /// Проверить, является ли команда временной (создана автоматически в рамках мероприятия)
    /// </summary>
    public bool IsTemporaryTeam()
        => !OwnerId.HasValue;
}
