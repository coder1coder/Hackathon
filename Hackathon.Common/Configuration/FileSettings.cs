namespace Hackathon.Common.Configuration;

public class FileSettings
{
    /// <summary>
    /// Периодичность работы службы удаления файлов в часах
    /// </summary>
    public int UnusedFilesDeleteJobFrequencyInHours { get; set; } = 24;
}
