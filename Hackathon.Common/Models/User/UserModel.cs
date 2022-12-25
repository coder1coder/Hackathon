using System;

namespace Hackathon.Common.Models.User
{
    public class UserModel: UserShortModel
    {
        public string PasswordHash { get; set; }

        /// <summary>
        /// Email пользователя
        /// </summary>
        public UserEmailModel Email { get; set; } = new();

        public GoogleAccountModel GoogleAccount { get; set; }

        /// <summary>
        /// Дата добавления пользователя в команду
        /// </summary>
        public DateTime DateTimeAdd { get; set; } = new();
    }
}
