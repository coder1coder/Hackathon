using Hackathon.Common.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hackathon.DAL.Configurations
{
    public class TeamEntityConfiguration : IEntityTypeConfiguration<TeamEntity>
    {
        public void Configure(EntityTypeBuilder<TeamEntity> builder)
        {
            builder.HasKey(x => x.Id);
            
            builder
                .HasIndex(x => x.Name)
                .IsUnique();

            builder
                .Property(x => x.Name)
                .IsRequired();



        }
    }
}