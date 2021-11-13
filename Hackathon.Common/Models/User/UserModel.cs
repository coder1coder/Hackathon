namespace Hackathon.Common.Models.User
{
    public class UserModel
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
    }
}