using System.Collections.Generic;

namespace Hackathon.DAL.Entities
{
    public class TeamEntity : BaseEntity
    {
        public string Name { get; set; }
        public EventEntity Event { get; set; }
        public long? EventId { get; set; }

        public List<UserEntity> Users { get; set; } = new ();

        public ProjectEntity Project { get; set; }
        public long? ProjectId { get; set; }
    }
}