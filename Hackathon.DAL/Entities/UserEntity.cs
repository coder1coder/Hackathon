using System.Collections.Generic;
using Hackathon.Common.Models.User;

namespace Hackathon.DAL.Entities
{
    public class UserEntity: BaseEntity
    {
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }

        public UserRole Role { get; set; }

        public List<TeamEntity> Teams { get; set; } = new ();
    }
}