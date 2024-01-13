using Hackathon.DAL.Entities.Interfaces;
using Hackathon.DAL.Extensions;
using Hackathon.Informing.DAL.Configurations;
using Hackathon.Informing.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.Informing.DAL;

/// <summary>
/// Контекст информирования
/// </summary>
public class InformingDbContext: DbContext
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
        modelBuilder.ApplyConfiguration(new NotificationEntityConfiguration());
        modelBuilder.ApplyConfiguration(new TemplateEntityConfiguration());
        
        modelBuilder.ApplyGlobalFilters<ISoftDeletable>(e => !e.IsDeleted);
    }
}
