using System;

namespace Hackathon.Common.Models.Event;

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
