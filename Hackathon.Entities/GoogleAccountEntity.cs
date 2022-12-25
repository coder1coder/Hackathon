using Hackathon.Entities.User;

namespace Hackathon.Entities;

public class GoogleAccountEntity
{
    public string Id { get; set; }
    public string FullName { get; set; }
    public string GivenName { get; set; }
    public string ImageUrl { get; set; }

    /// <summary>
    /// Email пользователя
    /// </summary>
    public string Email { get; set; }
    public string AccessToken { get; set; }
    public long ExpiresAt { get; set; }
    public long ExpiresIn { get; set; }
    public long FirstIssuedAt { get; set; }
    public string TokenId { get; set; }
    public string LoginHint { get; set; }

    public int UserId { get; set; }
    public UserEntity User { get; set; }
}
