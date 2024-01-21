using Hackathon.DAL;
using Hackathon.Informing.DAL.Configurations;
using Hackathon.Informing.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.Informing.DAL;

/// <summary>
/// Контекст информирования
/// </summary>
public class InformingDbContext: BaseDbContext
{
    /// <summary>
    /// Уведомления
    /// </summary>
    public DbSet<NotificationEntity> Notifications { get; set; }
    
    /// <summary>
    /// Шаблоны
    /// </summary>
    public DbSet<TemplateEntity> Templates { get; set; }
    
    public InformingDbContext(DbContextOptions<InformingDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfiguration(new NotificationEntityConfiguration());
        modelBuilder.ApplyConfiguration(new TemplateEntityConfiguration());
    }
}
