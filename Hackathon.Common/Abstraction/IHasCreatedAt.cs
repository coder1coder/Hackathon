using System;

namespace Hackathon.Common.Abstraction;

public interface IHasCreatedAt
{
    /// <summary>
    /// Дата и время создания сущности
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
