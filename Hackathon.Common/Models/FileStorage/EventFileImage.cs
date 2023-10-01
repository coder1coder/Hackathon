using SixLabors.ImageSharp;
using System;
using System.IO;

namespace Hackathon.Common.Models.FileStorage;

public class EventFileImage : IFileImage
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

    private EventFileImage()
    {

    }

    private EventFileImage(int width, int height, long length, string extension)
    {
        Width = width;
        Height = height;
        Length = length;
        Extension = extension;
    }

    public static EventFileImage FromStream(Stream stream, string fileName)
    {
        try
        {
            var image = Image.Load(stream);
            return new EventFileImage(image.Width, image.Height, stream.Length,
                Path.GetExtension(fileName)?.ToLower());
        }
        catch (Exception)
        {
            return new EventFileImage();
        }
    }
}