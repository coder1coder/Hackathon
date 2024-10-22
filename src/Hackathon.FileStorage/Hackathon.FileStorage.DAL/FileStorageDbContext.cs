using Hackathon.DAL;
using Hackathon.FileStorage.DAL.Configurations;
using Hackathon.FileStorage.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.FileStorage.DAL;

public class FileStorageDbContext: BaseDbContext
{
    public DbSet<FileStorageEntity> StorageFiles { get; set; }
    
    public FileStorageDbContext(DbContextOptions<FileStorageDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new FileStorageEntityConfiguration());
    }
    
}
