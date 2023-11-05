using Hackathon.Chats.DAL.Configurations;
using Hackathon.Chats.DAL.Entities;
using Hackathon.DAL;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.Chats.DAL;

// ReSharper disable UnusedAutoPropertyAccessor.Global
public class ChatsDbContext: BaseDbContext
{
    /// <summary>
    /// Сообщения
    /// </summary>
    public DbSet<ChatMessageEntity> ChatMessages { get; set; }

    public ChatsDbContext(DbContextOptions<ChatsDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ChatMessageEntityConfiguration).Assembly);
    }
}
