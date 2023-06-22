using Hackathon.Common.Models.Friend;
using System;

namespace Hackathon.DAL.Entities;

/// <summary>
/// Сущность дружбы
/// </summary>
public class FriendshipEntity
{
    /// <summary>
    /// Идентификатор дружбы
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Автор предложения дружбы
    /// </summary>
    public long ProposerId { get; set; }

    /// <summary>
    /// Пользователь кому адресовано предложение дружить
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    /// Статус дружбы
    /// </summary>
    public FriendshipStatus Status { get; set; }
}
