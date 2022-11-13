using Hackathon.Common.Models.Team;
using Hackathon.Entities.Interfaces;
﻿using Hackathon.Entities.User;

namespace Hackathon.Entities
{
    /// <summary>
    /// Команда
    /// </summary>
    public class TeamEntity : BaseEntity, ISoftDeletable
    {
        /// <summary>
        /// Наименование
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// События
        /// </summary>
        public ICollection<EventEntity> Events { get; set; } = new List<EventEntity>();

        /// <summary>
        /// Участники
        /// </summary>
        public ICollection<MemberTeamEntity> Members { get; set; } = new List<MemberTeamEntity>();
        
        /// <summary>
        /// Владелец команды
        /// </summary>
        public UserEntity? Owner { get; set; }
        
        /// <summary>
        /// Идентификатор владельца команды
        /// </summary>
        public long? OwnerId { get; set; }

        /// <summary>
        /// Признак удаления
        /// </summary>
        public bool IsDeleted { get; set; } = false;

        /// <summary>
        /// Тип команды
        /// </summary>
        public TeamType Type { get; set; }
    }
}
