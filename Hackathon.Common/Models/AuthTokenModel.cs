using System;

namespace Hackathon.Common.Models
{
    public class AuthTokenModel
    {
        public long UserId { get; set; }
        public string Token { get; set; }
        public DateTime? Expires { get; set; }
    }
}