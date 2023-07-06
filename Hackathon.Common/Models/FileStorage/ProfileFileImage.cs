using System.IO;

namespace Hackathon.Common.Models.FileStorage
{
    public class ProfileFileImage : FileImage
    {
        protected override int MinHeight => 150;
        protected override int MinWidth => 150;

        public ProfileFileImage(Stream stream, string fileName, long minLength, long maxLenght) 
            : base(stream, fileName, minLength, maxLenght)
        {
        }
    }
}
