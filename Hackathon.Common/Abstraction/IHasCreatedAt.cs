using System;

namespace Hackathon.Common.Abstraction;

/// <summary>
/// Сущность имеет дату и время создания
/// </summary>
public interface IHasCreatedAt
{
    /// <summary>
    /// Дата и время создания сущности
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
