using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hackathon.DAL.Entities
{
    public class ProjectEntity: BaseEntity, IEntityTypeConfiguration<ProjectEntity>
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public TeamEventEntity TeamEvent { get; set; }
        public long TeamEventId { get; set; }

        public void Configure(EntityTypeBuilder<ProjectEntity> builder)
        {
            builder.ToTable("Projects");

            builder
                .Property(x => x.Name)
                .IsRequired();

            builder
                .Property(x => x.Description)
                .HasMaxLength(1000);

        }
    }
}