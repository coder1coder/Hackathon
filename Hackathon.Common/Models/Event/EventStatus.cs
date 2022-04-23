using System;

namespace Hackathon.Common.Models.Event
{
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
        /// Разработка проекта
        /// </summary>
        /// <remarks></remarks>
        Development = 3,
        
        /// <summary>
        /// Подготовка к презентации
        /// </summary>
        Prepare = 4,
        
        /// <summary>
        /// Презентация проекта
        /// </summary>
        /// <remarks>Капитаны команд представляют свой проект</remarks>
        Presentation = 5,
        
        /// <summary>
        /// Вынесение решения
        /// </summary>
        /// <remarks>Организатор принимает решение о победителе</remarks>
        Decision = 6,
        
        /// <summary>
        /// Награждение
        /// </summary>
        /// <remarks>Объявление победителя</remarks>
        Award = 7,
        
        /// <summary>
        /// Событие завершено
        /// </summary>
        /// <remarks>Все доступные ранее операции запрещены</remarks>
        Finished = 8,
    }

    public static class EventStatusExtensions
    {
        public static string ToDefaultChangedEventStatusMessage(this EventStatus status, string eventName)
            => status switch
            {
                EventStatus.Draft => $"Событие '{eventName}' сохранено как черновик",
                EventStatus.Published => $"Событие '{eventName}' опубликовано",
                EventStatus.Started => $"Событие '{eventName}' началось",
                EventStatus.Development => $"{eventName}: начался этап разработки",
                EventStatus.Prepare => $"{eventName}: этап подготовки к презентации проекта",
                EventStatus.Presentation => $"{eventName}: презентация проекта началась",
                EventStatus.Decision => $"{eventName}: выбирается победитель",
                EventStatus.Award => $"{eventName}: Награждение",
                EventStatus.Finished => $"Событие '{eventName}' завершено",
                
                _ => throw new ArgumentOutOfRangeException(nameof(status), status, null)
            };
    }
}