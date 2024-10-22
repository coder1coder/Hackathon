namespace Hackathon.DAL.Entities;

/// <summary>
/// Базовая сущность
/// </summary>
public abstract class BaseEntity
{
    /// <summary>
    /// Идентификатор сущности
    /// </summary>
    public long Id { get; set; }
}
