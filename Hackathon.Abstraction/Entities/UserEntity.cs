﻿using Hackathon.Common.Models.User;
using Hackathon.DAL.Entities;

namespace Hackathon.Abstraction.Entities
{
    public class UserEntity : BaseEntity
    {
        public string? UserName { get; set; }
        public string? PasswordHash { get; set; }
        public string? Email { get; set; }
        public string? FullName { get; set; }

        /// <summary>
        /// Идентификатор пользователя социальной сети Google
        /// </summary>
        public string? GoogleAccountId { get; set; }

        public GoogleAccountEntity? GoogleAccount { get; set; }

        public UserRole Role { get; set; }

        public List<TeamEntity> Teams { get; set; } = new();
    }
}