namespace Hackathon.Common.Models
{
    public class AuthTokenModel
    {
        public long UserId { get; set; }
        public string Token { get; set; }
        public long Expires { get; set; }
    }
}