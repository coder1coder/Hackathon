using Hackathon.Chats.DAL.Configurations;
using Hackathon.Chats.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.Chats.DAL;

// ReSharper disable UnusedAutoPropertyAccessor.Global
public class ChatsDbContext: DbContext
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
