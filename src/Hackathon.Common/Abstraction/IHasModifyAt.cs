using System;

namespace Hackathon.Common.Abstraction;

/// <summary>
/// Сущность имеет дату и время изменения
/// </summary>
public interface IHasModifyAt
{
    /// <summary>
    /// Дата и время изменения сущености
    /// </summary>
    public DateTime? ModifyAt { get; set; }
}
