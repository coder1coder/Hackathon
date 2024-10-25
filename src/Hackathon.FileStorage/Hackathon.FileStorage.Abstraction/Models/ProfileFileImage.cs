namespace Hackathon.FileStorage.Abstraction.Models;

/// <summary>
/// Изображение профиля пользователя
/// </summary>
/// <param name="Width">Ширина</param>
/// <param name="Height">Высота</param>
/// <param name="Length">Размер</param>
/// <param name="Extension">Расширение файла</param>
public record ProfileFileImage(int Width, int Height, long Length, string Extension)
    : FileImage(Width, Height, Length, Extension);
