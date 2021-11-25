using System;
using System.Reflection;
using Hackathon.Common.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.DAL
{
    public class ApplicationDbContext: DbContext
    {
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<EventEntity> Events { get; set; }
        public DbSet<TeamEntity> Teams { get; set; }
        public DbSet<ProjectEntity> Projects { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            var assembly = Assembly.GetAssembly(typeof(ApplicationDbContext));
            if (assembly != null)
                modelBuilder.ApplyConfigurationsFromAssembly(assembly);
        }
    }
}