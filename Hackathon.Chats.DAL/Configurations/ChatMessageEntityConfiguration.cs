using Hackathon.Chats.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hackathon.Chats.DAL.Configurations;

public class ChatMessageEntityConfiguration: IEntityTypeConfiguration<ChatMessageEntity>
{
    public void Configure(EntityTypeBuilder<ChatMessageEntity> builder)
    {
        builder.ToTable("ChatMessages");
        builder.HasKey(x => x.MessageId);
        builder.HasIndex(x => new { x.ChatType, x.ChatId });
    }
}
