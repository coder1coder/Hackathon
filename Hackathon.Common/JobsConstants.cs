using System;

namespace Hackathon.Common;

public static class JobsConstants
{
    /// <summary>
    /// Периодичность работы службы обновления статуса начатого мероприятия
    /// </summary>
    public static TimeSpan StartedEventUpdateStatusJobFrequency = TimeSpan.FromMinutes(1);

    /// <summary>
    /// Периодичность работы службы удаления файлов в часах
    /// </summary>
    public static int UnusedFilesDeleteJobFrequency = 48;
}
