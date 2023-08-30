using Hackathon.Common.Abstraction;
using Hackathon.DAL.Entities;
using Hackathon.DAL.Entities.Event;
using Hackathon.DAL.Entities.Interfaces;
using Hackathon.DAL.Entities.User;
using Hackathon.DAL.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hackathon.DAL.Entities.ApprovalApplications;

namespace Hackathon.DAL;

public class ApplicationDbContext: DbContext
{
    /// <summary>
    /// Пользователи
    /// </summary>
    public DbSet<UserEntity> Users { get; set; }

    /// <summary>
    /// Реакции пользователей
    /// </summary>
    public DbSet<UserReactionEntity> UserReactions { get; set; }

    /// <summary>
    /// Сведения Google аккаунтов
    /// </summary>
    public DbSet<GoogleAccountEntity> GoogleAccounts { get; set; }

    /// <summary>
    /// Мероприятия
    /// </summary>
    public DbSet<EventEntity> Events { get; set; }

    /// <summary>
    /// Команды
    /// </summary>
    public DbSet<TeamEntity> Teams { get; set; }

    /// <summary>
    /// Запросы на вступление в команду
    /// </summary>
    public DbSet<TeamJoinRequestEntity> TeamJoinRequests { get; set; }

    public DbSet<ProjectEntity> Projects { get; set; }
    public DbSet<NotificationEntity> Notifications { get; set; }
    public DbSet<FileStorageEntity> StorageFiles { get; set; }
    public DbSet<EventLogEntity> EventLog { get; set; }
    public DbSet<FriendshipEntity> Friendships { get; set; }

    /// <summary>
    /// Соглашения об участии в мероприятии
    /// </summary>
    public DbSet<EventAgreementEntity> EventAgreements { get; set; }
    public DbSet<EventAgreementUserEntity> EventAgreementUsers { get; set; }

    /// <summary>
    /// Запросы на подтверждение Email пользователя
    /// </summary>
    public DbSet<EmailConfirmationRequestEntity> EmailConfirmations { get; set; }

    /// <summary>
    /// Заявки на согласование
    /// </summary>
    public DbSet<ApprovalApplicationEntity> ApprovalApplications { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            modelBuilder.ApplyGlobalFilters<ISoftDeletable>(e => !e.IsDeleted);
    }

    public override int SaveChanges()
    {
        SetDates();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        SetDates();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void SetDates()
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e =>
                (e.Entity is IHasCreatedAt or IHasModifyAt) && (e.State is EntityState.Added or EntityState.Modified));

        foreach (var entityEntry in entries)
        {
            if (entityEntry.State == EntityState.Added && entityEntry.Entity is IHasCreatedAt createdEntity)
                createdEntity.CreatedAt = DateTime.UtcNow;

            if (entityEntry.State == EntityState.Modified && entityEntry.Entity is IHasModifyAt modifiedEntity)
                modifiedEntity.ModifyAt = DateTime.UtcNow;
        }
    }
}
