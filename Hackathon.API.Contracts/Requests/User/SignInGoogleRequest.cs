namespace Hackathon.Contracts.Requests.User
{
    public class SignInGoogleRequest
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string GiveName { get; set; }
        public string ImageUrl { get; set; }
        public string Email { get; set; }
        public string AccessToken { get; set; }
        public long ExpiresAt { get; set; }
        public long ExpiresIn { get; set; }
        public long FirstIssuedAt { get; set; }
        public string TokenId { get; set; }
        public string LoginHint { get; set; }
        public bool IsLoggedIn { get; set; }
    }
}
