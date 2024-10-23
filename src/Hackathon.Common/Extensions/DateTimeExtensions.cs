using System;

namespace Hackathon.Common.Extensions;

public static class DateTimeExtensions
{
    /// <summary>
    /// Преобразовать в UTC и исключить секунды и миллисекунды
    /// </summary>
    /// <param name="dateTime">Дата и время</param>
    public static DateTime ToUtcWithoutSeconds(this DateTime dateTime)
    {
        var utc = dateTime.ToUniversalTime();
        return utc.Date.AddHours(utc.Hour).AddMinutes(utc.Minute);
    }
}
