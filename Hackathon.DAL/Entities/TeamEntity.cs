using System.Collections.Generic;

namespace Hackathon.DAL.Entities
{
    public class TeamEntity : BaseEntity
    {
        public string Name { get; set; }

        public List<TeamEventEntity> TeamEvents { get; set; } = new ();
        public List<UserEntity> Users { get; set; } = new ();
        public UserEntity Owner { get; set; }
        public long? OwnerId { get; set; }
    }
}