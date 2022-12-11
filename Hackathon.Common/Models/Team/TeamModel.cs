using System;
using Hackathon.Common.Models.User;

namespace Hackathon.Common.Models.Team
{
    public class TeamModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public UserModel[] Members { get; set; } = Array.Empty<UserModel>();
        public UserModel Owner { get; set; }
        public long? OwnerId { get; set; }
    }
}
