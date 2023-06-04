using System;

namespace Hackathon.Common;

public static class JobsConstants
{
    /// <summary>
    /// Периодичность работы службы обновления статуса начатого мероприятия
    /// </summary>
    public static TimeSpan StartedEventUpdateStatusJobFrequency = TimeSpan.FromMinutes(1);
}
