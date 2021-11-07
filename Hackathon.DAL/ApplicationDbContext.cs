using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.DAL
{
    public class ApplicationDbContext: DbContext
    {
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