using Hackathon.Common.Models.User;

namespace Hackathon.Entities
{
    /// <summary>
    /// Пользователь
    /// </summary>
    public class UserEntity: BaseEntity
    {
        public string? UserName { get; set; }
        public string? PasswordHash { get; set; }
        public string? Email { get; set; }
        public string? FullName { get; set; }
        
        /// <summary>
        /// Идентификатор пользователя социальной сети Google
        /// </summary>
        public string? GoogleAccountId { get; set; }
        public GoogleAccountEntity? GoogleAccount { get; set; }

        public UserRole Role { get; set; }

        /// <summary>
        /// Команды в которых участвует пользователь
        /// </summary>
        public List<MemberTeamEntity> Teams { get; set; } = new ();
        public Guid? ProfileImageId { get; set; }
    }
}