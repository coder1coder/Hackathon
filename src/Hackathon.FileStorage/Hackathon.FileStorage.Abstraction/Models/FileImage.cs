namespace Hackathon.FileStorage.Abstraction.Models;

public record FileImage(int Width, int Height, long Length, string Extension) : IFileImage
{
    /// <summary>
    /// Ширина в px
    /// </summary>
    public int Width { get; } = Width;

    /// <summary>
    /// Высота в px
    /// </summary>
    public int Height { get; } = Height;

    /// <summary>
    /// Размер в байтах
    /// </summary>
    public long Length { get; } = Length;

    /// <summary>
    /// Расширение файла
    /// </summary>
    public string Extension { get; } = Extension;
}
