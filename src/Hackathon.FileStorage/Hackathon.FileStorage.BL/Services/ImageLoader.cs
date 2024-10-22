using System;
using System.IO;
using System.Threading.Tasks;
using Hackathon.FileStorage.Abstraction.Models;
using SixLabors.ImageSharp;

namespace Hackathon.FileStorage.BL.Services;

public sealed class ImageLoader
{
    public static async Task<TFileImage> LoadFromStreamAsync<TFileImage>(Stream stream, string fileName, Func<IFileImage, TFileImage> mapper) 
        where TFileImage: IFileImage
    {
        try
        {
            var image = await Image.LoadAsync(stream);

            var loadedFileImage = new FileImage(image.Width, image.Height, stream.Length, 
                Path.GetExtension(fileName)?.ToLower());

            return mapper.Invoke(loadedFileImage);
        }
        catch (Exception)
        {
            throw new AggregateException("Couldn't load image from stream");
        }
    }
}
