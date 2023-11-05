namespace Hackathon.FileStorage.Configuration;

public class FileSettings
{
    /// <summary>
    /// Настройки изображений профиля
    /// </summary>
    public ImageSettings ProfileFileImage { get; set; }

    /// <summary>
    /// Настройки изображений события
    /// </summary>
    public ImageSettings EventFileImage { get; set; }
}
