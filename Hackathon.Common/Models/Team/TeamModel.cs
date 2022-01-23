using System.Collections.Generic;
using Hackathon.Common.Models.User;

namespace Hackathon.Common.Models.Team
{
    public class TeamModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public List<UserModel> Users { get; set; }

        public UserModel Owner { get; set; }
        public long? OwnerId { get; set; }
        public List<TeamEventModel> TeamEvents { get; set; }
    }
}