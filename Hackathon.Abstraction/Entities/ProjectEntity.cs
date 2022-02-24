using Hackathon.DAL.Entities;

namespace Hackathon.Abstraction.Entities
{
    public class ProjectEntity: BaseEntity
    {
        public string? Name { get; set; }
        public string? Description { get; set; }

        public TeamEventEntity? TeamEvent { get; set; }
        public long TeamEventId { get; set; }
    }
}