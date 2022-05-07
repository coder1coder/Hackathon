using System.Linq;
using Hackathon.Common.Models.Event;

namespace Hackathon.Common.Extensions;

public static class EventExtensions
{
    public static EventListItem ToListItem(this EventModel model) => new()
    {
        Id = model.Id,
        Name = model.Name,
        OwnerId = model.OwnerId,
        OwnerName = model.Owner?.ToString(),
        Start = model.Start,
        Status = model.Status,
        MaxEventMembers = model.MaxEventMembers,
        MinTeamMembers = model.MinTeamMembers,
        IsCreateTeamsAutomatically = model.IsCreateTeamsAutomatically,

        TeamsCount = model.Teams?.Count ?? 0,
        MembersCount = model.Teams?.Sum(x => x.Members?.Count) ?? 0
    };
}