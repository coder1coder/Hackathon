using System;
using System.Collections.Generic;
using Hackathon.Common.Models.Event;

namespace Hackathon.Contracts.Requests.Event
{
    /// <summary>
    /// Контракт создания нового события
    /// </summary>
    public class CreateEventRequest
    {
        /// <summary>
        /// Наименование события
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Описание события
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Дата и время начала
        /// </summary>
        public DateTime Start { get; set; }

        /// <summary>
        /// Продолжительность формированя команд в минутах
        /// </summary>
        public int MemberRegistrationMinutes { get; set; }

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
        public List<ChangeEventStatusMessage> ChangeEventStatusMessages { get; set; }
        
        /// <summary>
        /// Награда, призовой фонд
        /// </summary>
        public string Award { get; set; }
    }
}