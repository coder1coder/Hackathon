namespace Hackathon.Common.Models.User;

public class GoogleAccountModel
{
    /// <summary>
    /// Идентификатор учетной записи Google
    /// </summary>
    public string Id { get; set; }
    
    /// <summary>
    /// Полное имя учетной записи
    /// </summary>
    public string FullName { get; set; }
    
    /// <summary>
    /// Email пользователя
    /// </summary>
    public string Email { get; set; }
    
    /// <summary>
    /// Ссылка на изображение учетной записи Google
    /// </summary>
    public string ImageUrl { get; set; }

    public bool Equals(GoogleAccountModel other)
        => Id == other.Id 
           && FullName == other.FullName 
           && Email == other.Email 
           && ImageUrl == other.ImageUrl;
}
