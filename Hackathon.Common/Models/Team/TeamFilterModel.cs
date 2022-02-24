﻿namespace Hackathon.Common.Models.Team
{
    public class TeamFilterModel
    {
        public long[] Ids { get; set; }

        public string Name { get; set; }

        public string Owner { get; set; }

        public long? QuantityUsersFrom { get; set; }

        public long? QuantityUsersTo { get; set; }

        public long? EventId { get; set; }

        public long? ProjectId { get; set; }

        public long? OwnerId { get; set; }

    }
}