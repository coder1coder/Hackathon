using Hackathon.DAL;
using Hackathon.Logbook.DAL.Configurations;
using Hackathon.Logbook.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.Logbook.DAL;

public class LogbookDbContext: BaseDbContext
{
    public DbSet<EventLogEntity> EventLog { get; set; }
    
    public LogbookDbContext(DbContextOptions<LogbookDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfiguration(new EventLogEntityConfiguration());
    }
}
