using Hackathon.Abstraction.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.DAL
{
    public class ApplicationDbContext: DbContext
    {
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<GoogleAccountEntity> GoogleAccounts { get; set; }

        public DbSet<EventEntity> Events { get; set; }
        public DbSet<TeamEntity> Teams { get; set; }
        public DbSet<ProjectEntity> Projects { get; set; }
        public DbSet<NotificationEntity> Notifications { get; set; }

        public DbSet<FileStorageEntity> StorageFiles { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}