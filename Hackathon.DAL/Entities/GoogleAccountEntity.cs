using Hackathon.DAL.Entities.User;

namespace Hackathon.DAL.Entities;

public class GoogleAccountEntity
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

    /// <summary>
    /// Идентификатор пользователя системы
    /// </summary>
    public int UserId { get; set; }
    
    /// <summary>
    /// Пользователь системы связанный с учетной записью Google
    /// </summary>
    public UserEntity User { get; set; }
}
