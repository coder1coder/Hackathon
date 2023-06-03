namespace Hackathon.Common.Models.Event;

/// <summary>
/// Статус мероприятия
/// </summary>
public enum EventStatus
{
    /// <summary>
    /// Черновик
    /// </summary>
    /// <remarks>Могут вноситься любые правки</remarks>
    Draft = default,

    /// <summary>
    /// Опубликовано
    /// </summary>
    /// <remarks>
    /// Нельзя вносить изменения в общие параметры.
    /// Регистрация участников, команд
    /// </remarks>
    Published = 1,

    /// <summary>
    /// Событие начато
    /// </summary>
    /// <remarks>Регистрация закрыта. Организатор приветствует участников</remarks>
    Started = 2,

    /// <summary>
    /// Событие завершено
    /// </summary>
    /// <remarks>Все доступные ранее операции запрещены</remarks>
    Finished = 8
}
