﻿using System;
using System.Collections.Generic;
using Hackathon.Common.Models.Team;
using Hackathon.Common.Models.User;

namespace Hackathon.Common.Models.Event
{
    public class EventModel
    {
        /// <summary>
        /// Идентификатор события
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Наименование события
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Дата и время начала
        /// </summary>
        public DateTime Start { get; set; }

        /// <summary>
        /// Этапы
        /// </summary>
        public List<EventStage> Stages { get; set; }= new();

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
        public List<ChangeEventStatusMessage> ChangeEventStatusMessages { get; set; } = new();

        /// <summary>
        /// Статус события
        /// </summary>
        public EventStatus Status { get; set; }

        public long UserId { get; set; }
        public UserModel User { get; set; }

        /// <summary>
        /// Список команд связанных с событием
        /// </summary>
        public List<TeamEventModel> TeamEvents { get; set; } = new();
    }
}