using Hackathon.Common.Models.Base;

namespace Hackathon.Common.Models.Team
{
    public class TeamFilterModel: IFilterModel
    {
        public long[] Ids { get; set; }
        public string Name { get; set; }
        public long? EventId { get; set; }
        public long? ProjectId { get; set; }
    }
}