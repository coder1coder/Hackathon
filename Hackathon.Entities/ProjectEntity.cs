namespace Hackathon.Entities
{
    public class ProjectEntity: BaseEntity
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        
        public long EventId { get; set; }
        public EventEntity? Event { get; set; }

        public long TeamId { get; set; }
        public TeamEntity? Team { get; set; }
    }
}