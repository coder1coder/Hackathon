using System.Collections.Generic;
using Hackathon.Common.Models.Event;
using Hackathon.Common.Models.User;

namespace Hackathon.Common.Models.Team
{
    public class TeamModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public List<UserModel> Members { get; set; } = new();
        public UserModel Owner { get; set; }
        public long? OwnerId { get; set; }
        public List<EventModel> Events { get; set; } = new();
    }
}
