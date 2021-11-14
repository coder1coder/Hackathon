using System.Collections.Generic;
using Hackathon.Common.Models.Event;
using Hackathon.Common.Models.User;

namespace Hackathon.Common.Models.Team
{
    public class TeamModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long EventId { get; set; }

        public EventModel Event { get; set; }
        public List<UserModel> Users { get; set; }
    }
}