using Hackathon.Common.Models.User;
using Hackathon.Entities.Interfaces;

namespace Hackathon.Entities
{
    /// <summary>
    /// Пользователь
    /// </summary>
    public class UserEntity: BaseEntity, ISoftDeletable
    {
        /// <summary>
        /// Логин пользователя
        /// </summary>
        public string? UserName { get; set; }

        /// <summary>
        /// Пароль пользователя
        /// </summary>
        public string? PasswordHash { get; set; }

        /// <summary>
        /// Электронная почта пользователя
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Полное наименование пользователя
        /// </summary>
        public string? FullName { get; set; }
        
        /// <summary>
        /// Идентификатор пользователя социальной сети Google
        /// </summary>
        public string? GoogleAccountId { get; set; }

        public GoogleAccountEntity? GoogleAccount { get; set; }

        /// <summary>
        /// Роль пользователя
        /// </summary>
        public UserRole Role { get; set; }

        /// <summary>
        /// Команды в которых участвует пользователь
        /// </summary>
        public List<MemberTeamEntity> Teams { get; set; } = new ();

        /// <summary>
        /// Идентификатор аватара профиля в файловом хранилище
        /// </summary>
        public Guid? ProfileImageId { get; set; }

        /// <summary>
        /// Признак удаления
        /// </summary>
        public bool IsDeleted { get; set; } = false;
    }
}