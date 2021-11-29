using System.Collections.Generic;
using Hackathon.Common.Models.Team;

namespace Hackathon.Common.Models.Event
{
    public class EventFullModel: EventModel
    {
        public List<TeamModel> Teams { get; set; } = new ();
    }
}