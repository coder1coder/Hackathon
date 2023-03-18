using Hackathon.Common.ErrorMessages;
using Hackathon.Common.Models.Event;

namespace Hackathon.BL.Validation.Event;

public class ChangeEventStatusValidator
{
    public static (bool, string) ValidateAsync(EventModel eventModel, EventStatus newStatus)
    {
        var canChangeStatus = newStatus == EventStatus.Finished || (int)eventModel.Status == (int)newStatus - 1;

        if (!canChangeStatus)
            return (false, EventMessages.CantSetEventStatus);

        return newStatus switch
        {
            EventStatus.Started => IsEventStatusCanBeStarted(eventModel),

            _ => (true, string.Empty)
        };
    }

    private static (bool, string) IsEventStatusCanBeStarted(EventModel eventModel)
    {
        var totalMembers = eventModel.Teams.Sum(x => x.Members?.Length);
        var minimalMembers = eventModel.MinTeamMembers * 2;

        return totalMembers < minimalMembers ?
            (false, "Для того, чтобы начать событие необходимо набрать минимальное количество участников")
            : (true, string.Empty);
    }

}
