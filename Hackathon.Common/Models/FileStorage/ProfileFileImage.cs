using System.IO;

namespace Hackathon.Common.Models.FileStorage
{
    public class ProfileFileImage : FileImage
    {
        public override int MinHeight => 150;
        public override int MinWidth => 150;

        public ProfileFileImage(Stream stream, string fileName, long minLength, long maxLenght) 
            : base(stream, fileName, minLength, maxLenght)
        {
        }
    }
}
