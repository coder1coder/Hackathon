using System;
using System.Collections.Generic;

namespace Hackathon.Common.Models.Event
{
    /// <summary>
    /// Модель нового события
    /// </summary>
    public class CreateEventModel
    {
        /// <summary>
        /// Наименование события
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Дата и время начала
        /// </summary>
        private DateTime _start;
        public DateTime Start
        {
            get => _start;
            set => _start = DateTime.SpecifyKind(value, DateTimeKind.Utc);
        }

        /// <summary>
        /// Продолжительность формированя команд в минутах
        /// </summary>
        public int MemberRegistrationMinutes { get; set; }

        /// <summary>
        /// Продолжительность этапа разработки в минутах
        /// </summary>
        public int DevelopmentMinutes { get; set; }

        /// <summary>
        /// Продолжительность выступления каждой команды в минутах
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
        public List<ChangeEventStatusMessage> ChangeEventStatusMessages { get; } = new();

        /// <summary>
        /// Кто создал событие
        /// </summary>
        public long UserId { get; set; }
    }
}