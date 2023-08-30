namespace Hackathon.Common.Models.Event;

/// <summary>
/// Статус мероприятия
/// </summary>
public enum EventStatus
{
    /// <summary>
    /// Черновик
    /// </summary>
    Draft = default,

    /// <summary>
    /// На модерации
    /// </summary>
    OnModeration = 3,

    /// <summary>
    /// Опубликовано
    /// </summary>
    /// <remarks>
    /// Нельзя вносить изменения в общие параметры
    /// </remarks>
    Published = 1,

    /// <summary>
    /// Событие начато
    /// </summary>
    /// <remarks>Регистрация закрыта</remarks>
    Started = 2,

    /// <summary>
    /// Событие завершено
    /// </summary>
    /// <remarks>Все доступные ранее операции запрещены</remarks>
    Finished = 8
}
