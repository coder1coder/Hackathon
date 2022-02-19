namespace Hackathon.Contracts.Requests.User
{
    public class SignInGoogleRequest
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public string GiveName { get; set; }

        public string ImageUrl { get; set; }

        public string Email { get; set; }

        public string AccessToken { get; set; }

        public int ExpiresAt { get; set; }

        public int ExpiresIn { get; set; }

        public int FirstIssuedAt { get; set; }

        public string TokenId { get; set; }

        public string LoginHint { get; set; }

        public bool IsLoggedIn { get; set; }
    }
}
