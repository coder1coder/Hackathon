using Hackathon.Common.Abstraction;
using Hackathon.Common.Models.Block;
using Hackathon.DAL.Entities.User;
using System;

namespace Hackathon.DAL.Entities.Block;

/// <summary>
/// Блокировка
/// </summary>
public class BlockingEntity : IHasCreatedAt
{
    /// <summary>
    /// Id
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Тип блокировки
    /// </summary>
    public BlockingType Type { get; set; }

    /// <summary>
    /// Причина блокировки
    /// </summary>
    public string Reason { get; set; }

    /// <summary>
    /// Id пользователя, который назначил блокировку
    /// </summary>
    public long AssignmentUserId { get; private set; }

    /// <summary>
    /// Id пользователя, на которого назначена блокировка
    /// </summary>
    public long TargetUserId { get; private set; }

    /// <summary>
    /// Дата, до которой действует
    /// </summary>
    public DateTime? ActionDate { get; set; }

    ///<inheritdoc/>
    public DateTime CreatedAt { get; set; }

    public UserEntity TargetUser { get; set; }
}
