using Hackathon.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hackathon.DAL.Configurations;

public class FileStorageEntityConfiguration: IEntityTypeConfiguration<FileStorageEntity>
{
    public void Configure(EntityTypeBuilder<FileStorageEntity> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.BucketName).IsRequired();
        builder.Property(x => x.FileName).IsRequired();
        builder.Property(x => x.FilePath).IsRequired();
    }
}