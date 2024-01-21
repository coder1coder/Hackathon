using Hackathon.Common.Models.Block;
using System;

namespace Hackathon.Contracts.Requests.Block;

/// <summary>
/// Контракт для создания новой блокировки пользователя
/// </summary>
public class CreateBlockRequest
{
    /// <summary>
    /// Тип блокировки
    /// </summary>
    public BlockingType Type { get; set; }

    /// <summary>
    /// Причина блокировки
    /// </summary>
    public string Reason { get; set; }

    /// <summary>
    /// Дата, до которой действует
    /// </summary>
    public DateTime? ActionDate { get; set; }

    /// <summary>
    /// Часы, сколько действует
    /// </summary>
    public int? ActionHours { get; set; }

    /// <summary>
    /// Id пользователя, на которого назначена блокировка
    /// </summary>
    public long TargetUserId { get; private set; }
}
