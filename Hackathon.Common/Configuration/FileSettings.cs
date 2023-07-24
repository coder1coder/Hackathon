namespace Hackathon.Common.Configuration;

public class FileSettings
{
    /// <summary>
    /// Периодичность работы службы удаления файлов в часах
    /// </summary>
    public int UnusedFilesDeleteJobFrequencyInHours { get; set; } = JobsConstants.UnusedFilesDeleteJobFrequency;

    /// <summary>
    /// Настройки изображений профиля
    /// </summary>
    public ImageSettings ProfileFileImage { get; set; }

    /// <summary>
    /// Настройки изображений события
    /// </summary>
    public ImageSettings EventFileImage { get; set; }
}
