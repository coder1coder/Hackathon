using System.Collections.Generic;

namespace Hackathon.Common.Entities
{
    public class UserEntity: BaseEntity
    {
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }

        public List<TeamEntity> Teams { get; set; }
    }
}