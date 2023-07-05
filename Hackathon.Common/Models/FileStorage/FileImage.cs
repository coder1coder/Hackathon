using BackendTools.Common.Models;
using ImageMagick;
using System;
using System.IO;
using System.Linq;

namespace Hackathon.Common.Models.FileStorage
{
    public abstract class FileImage
    {
        public const string ErrorSizeMessage = "Минимальный размер изображения должен быть: {0} x {1}";
        public const string ErrorLengthMessage = "Размер изображения должен быть в пределах от {0} байт до {1} МБ";
        public const string ErrorExtensionMessage = "Формат изображения должен быть: {0}";

        private string[] _allowedExtensions = new string[] { ".jpg", ".jpeg", ".png" };

        /// <summary>
        /// Минимальная ширина
        /// </summary>
        public abstract int MinWidth { get; }

        /// <summary>
        /// Минимальная высота
        /// </summary>
        public abstract int MinHeight { get; }

        /// <summary>
        /// Минимальный размер в байтах
        /// </summary>
        public long MinLength { get; }

        /// <summary>
        /// Максимальный размер в байтах
        /// </summary>
        public long MaxLength { get; }

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

        public FileImage(Stream stream, string fileName, long minLength, long maxLenght)
        {
            using (var imageFromStream = new MagickImage(stream))
            {
                Width = imageFromStream.Width;
                Height = imageFromStream.Height;
            }

            Length = stream.Length;
            MinLength = minLength;
            MaxLength = maxLenght;
            Extension = Path.GetExtension(fileName);
        }

        public Result IsValid()
        {
            if (_allowedExtensions.Any(ext => ext == Extension))
            {
                if (Length >= MinLength && Length <= MaxLength)
                {
                    if (Width >= MinWidth && Height >= MinHeight)
                        return Result.Success;
                    else
                        return Result.NotValid(GetErrorSizeMessage());
                }
                else
                    return Result.NotValid(GetErrorLengthMessage());
            }

            return Result.NotValid(GetErrorExtensionMessage());
        }

        private string GetErrorSizeMessage() => string.Format(ErrorSizeMessage, MinWidth, MinHeight);
        private string GetErrorLengthMessage() => string.Format(ErrorLengthMessage, MinLength, GetMB());
        private string GetErrorExtensionMessage() => string.Format(ErrorExtensionMessage, 
            string.Join(" ", _allowedExtensions));

        private double GetMB()
        {
            return Math.Round(MaxLength / 1024d);
        }
    }
}
