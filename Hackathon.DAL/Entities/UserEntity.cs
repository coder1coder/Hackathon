using System.Collections.Generic;

namespace Hackathon.DAL.Entities
{
    public class UserEntity
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }

        public List<TeamEntity> Teams { get; set; } = new ();

        public ICollection<EventEntity> Events { get; set; } = new List<EventEntity>();
    }
}