namespace Hackathon.FileStorage.Abstraction.Models;

/// <summary>
/// Изображение мероприятия
/// </summary>
/// <param name="Width">Ширина</param>
/// <param name="Height">Высота</param>
/// <param name="Length">Размер</param>
/// <param name="Extension">Расширение файла</param>
public record EventFileImage(int Width, int Height, long Length, string Extension)
    : FileImage(Width, Height, Length, Extension);
