using Hackathon.DAL.Entities;
using Hackathon.DAL.Entities.ApprovalApplications;
using Hackathon.DAL.Entities.Event;
using Hackathon.DAL.Entities.User;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.DAL;

public class ApplicationDbContext: BaseDbContext
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
    /// Связь пользователей с командами
    /// </summary>
    public DbSet<MemberTeamEntity> TeamMembers { get; set; }

    /// <summary>
    /// Запросы на вступление в команду
    /// </summary>
    public DbSet<TeamJoinRequestEntity> TeamJoinRequests { get; set; }

    public DbSet<ProjectEntity> Projects { get; set; }
    

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
    }
}
