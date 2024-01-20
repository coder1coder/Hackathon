using Hackathon.Common.Models.Block;
using Hackathon.DAL.Entities.User;
using System;

namespace Hackathon.DAL.Entities.Block;

public class BlockEntity
{
    /// <summary>
    /// Id
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Тип блокировки
    /// </summary>
    public BlockType Type { get; set; }

    /// <summary>
    /// Снята ли блокировка
    /// </summary>
    public bool IsRemove { get; set; }

    /// <summary>
    /// Причина блокировки
    /// </summary>
    public string Reason { get; set; }

    /// <summary>
    /// Дата создания
    /// </summary>
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// Дата, до которой действует
    /// </summary>
    public DateTime? ActionDate { get; set; }

    /// <summary>
    /// Часы, сколько действует
    /// </summary>
    public int? ActionHours { get; set; }

    /// <summary>
    /// Id администратора, который назначил блокировку
    /// </summary>
    public long AdministratorId { get; set; }

    /// <summary>
    /// Id пользователя, на которого назначили блокировку
    /// </summary>
    public long UserId { get; set; }

    public UserEntity User { get; set; }
}
