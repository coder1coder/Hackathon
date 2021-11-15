namespace Hackathon.Common.Entities
{
    public class ProjectEntity: BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public TeamEntity Team { get; set; }
        public long TeamId { get; set; }
    }
}