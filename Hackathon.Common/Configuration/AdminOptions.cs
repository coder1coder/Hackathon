namespace Hackathon.Common.Configuration
{
    public class AdminOptions
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int LifeTime { get; set; }
        public string Secret { get; set; }
        public string Name { get; set; }
        public string PasswordHash { get; set; }
    }
}