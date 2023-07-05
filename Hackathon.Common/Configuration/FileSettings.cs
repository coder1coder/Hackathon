namespace Hackathon.Common.Configuration;

public class FileSettings
{
    /// <summary>
    /// Периодичность работы службы удаления файлов в часах
    /// </summary>
    public int UnusedFilesDeleteJobFrequencyInHours { get; set; } = JobsConstants.UnusedFilesDeleteJobFrequency;

    /// <summary>
    /// Настройки файлов изображений
    /// </summary>
    public FileImageSettings FileImage { get; set; }
}
