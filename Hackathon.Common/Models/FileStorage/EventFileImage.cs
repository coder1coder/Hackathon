using ImageMagick;
using System.IO;

namespace Hackathon.Common.Models.FileStorage
{
    public class EventFileImage : FileImage
    {
        protected override int MinWidth => 500;
        protected override int MinHeight => 500;

        public EventFileImage(Stream stream, string fileName, long minLength, long maxLenght) 
            : base(stream, fileName, minLength, maxLenght)
        {
        }
    }
}
