using System;
using Hackathon.Common.Models.Notification;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hackathon.DAL.Entities;

public class NotificationEntity: IEntityTypeConfiguration<NotificationEntity>
{
    public Guid Id { get; set; }
    public NotificationType Type { get; set; }
    public long UserId { get; set; }
    public long OwnerId { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsRead { get; set; }
    public string Data { get; set; }

    public void Configure(EntityTypeBuilder<NotificationEntity> builder)
    {
        builder.ToTable("Notifications");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.Data)
            .HasColumnType("jsonb");
    }
}