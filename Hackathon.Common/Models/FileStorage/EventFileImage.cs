using ImageMagick;
using System.IO;

namespace Hackathon.Common.Models.FileStorage
{
    public class EventFileImage : FileImage
    {
        public override int MinWidth => 500;
        public override int MinHeight => 500;

        public EventFileImage(Stream stream, string fileName, long minLength, long maxLenght) 
            : base(stream, fileName, minLength, maxLenght)
        {
        }
    }
}
