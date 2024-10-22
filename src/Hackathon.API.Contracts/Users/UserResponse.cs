using System;
using Hackathon.Common.Models.Users;

namespace Hackathon.API.Contracts.Users;

public class UserResponse
{
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public long Id { get; set; }
    
    /// <summary>
    /// Имя пользователя
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// Параметры Email пользователя
    /// </summary>
    public UserEmailResponse Email { get; set; }
    
    /// <summary>
    /// Полное имя пользователя
    /// </summary>
    public string FullName { get; set; }

    /// <summary>
    /// Сведения об учетной записи Google
    /// </summary>
    public GoogleAccountModel GoogleAccount { get; set; }
    
    /// <summary>
    /// Роль пользователя
    /// </summary>
    public UserRole Role { get; set; }

    /// <summary>
    /// Идентификатор файла изображения профиля
    /// </summary>
    public Guid? ProfileImageId { get; set; }
}
