using System;

namespace Hackathon.Common.Models.User
{
    public class UserModel
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }

        public GoogleAccountModel GoogleAccount { get; set; }
        public UserRole Role { get; set; } = UserRole.Default;

        public Guid? ProfileImageId { get; set; }

        public override string ToString()
            => string.IsNullOrWhiteSpace(FullName)
                ? UserName
                : $"{FullName} ({UserName})";
    }
}