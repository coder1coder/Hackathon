namespace Hackathon.FileStorage.Abstraction.Models;

public interface IFileImage
{
    /// <summary>
    /// Ширина в px
    /// </summary>
    public int Width { get; }

    /// <summary>
    /// Высота в px
    /// </summary>
    public int Height { get; }

    /// <summary>
    /// Размер в байтах
    /// </summary>
    public long Length { get; }

    /// <summary>
    /// Расширение файла
    /// </summary>
    public string Extension { get; }
}