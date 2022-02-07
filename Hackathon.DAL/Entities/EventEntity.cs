using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Hackathon.Common.Models.Event;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hackathon.DAL.Entities
{
    /// <summary>
    /// Событие
    /// </summary>
    public class EventEntity: BaseEntity, IEntityTypeConfiguration<EventEntity>
    {
        /// <summary>
        /// Кто создал событие
        /// </summary>
        public long UserId { get; set; }
        public UserEntity User { get; set; }

        /// <summary>
        /// Наименование события
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Дата и время начала
        /// </summary>
        public DateTime Start { get; set; }

        /// <summary>
        /// Продолжительность формированя команд в минутах
        /// </summary>
        public int MemberRegistrationMinutes { get; set; }

        /// <summary>
        /// Статус события
        /// </summary>
        public EventStatus Status { get; set; }

        /// <summary>
        /// Продолжительность этапа разработки в минутах
        /// </summary>
        public int DevelopmentMinutes { get; set; }

        /// <summary>
        /// Продолжительность выступленя каждой команды в минутах
        /// </summary>
        public int TeamPresentationMinutes { get; set; }

        /// <summary>
        /// Максимальное количество участников
        /// </summary>
        public int MaxEventMembers { get; set; }

        /// <summary>
        /// Минимальное количество участников в команде
        /// </summary>
        public int MinTeamMembers { get; set; }

        /// <summary>
        /// создавать команды автоматически
        /// </summary>
        public bool IsCreateTeamsAutomatically { get; set; }

        /// <summary>
        /// Список сообщений высылаемых командам при смене статусов
        /// </summary>
        [Column(TypeName = "jsonb")]
        public ICollection<ChangeEventStatusMessage> ChangeEventStatusMessages { get; set; } = new List<ChangeEventStatusMessage>();

        /// <summary>
        /// Команды связанные с событием
        /// </summary>
        public ICollection<TeamEventEntity> TeamEvents { get; set; } = new List<TeamEventEntity>();

        public void Configure(EntityTypeBuilder<EventEntity> builder)
        {
            builder.ToTable("Events");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Start).IsRequired();
            builder.Property(x => x.Status).IsRequired();
            builder.Property(x => x.MemberRegistrationMinutes).IsRequired();
            builder.Property(x => x.DevelopmentMinutes).IsRequired();
            builder.Property(x => x.TeamPresentationMinutes).IsRequired();
            builder.Property(x => x.MinTeamMembers).IsRequired();
            builder.Property(x => x.MaxEventMembers).IsRequired();
            builder.Property(x => x.IsCreateTeamsAutomatically).IsRequired();
            builder.Property(x => x.UserId).IsRequired();

            builder.Property(x => x.ChangeEventStatusMessages);
        }
    }
}