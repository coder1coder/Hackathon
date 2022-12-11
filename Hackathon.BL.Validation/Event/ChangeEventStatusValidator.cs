using Hackathon.Common.Exceptions;
using Hackathon.Common.Models.Event;

namespace Hackathon.BL.Validation.Event;

public class ChangeEventStatusValidator
{
    public async Task<(bool isValid, string errorMessage)> ValidateAsync(EventModel eventModel, EventStatus newStatus)
    {
        return await Task.FromResult(Validate(eventModel, newStatus));
    }

    private (bool, string) Validate(EventModel eventModel, EventStatus newStatus)
    {
        var canChangeStatus = newStatus == EventStatus.Finished || (int)eventModel.Status == (int)newStatus - 1;

        if (!canChangeStatus)
            return (false, ErrorMessages.CantSetEventStatus);

        return newStatus switch
        {
            EventStatus.Started => IsEventStatusCanBeStarted(eventModel),

            _ => (true, string.Empty)
        };
    }

    private (bool, string) IsEventStatusCanBeStarted(EventModel eventModel)
    {
        var totalMembers = eventModel.Teams.Sum(x => x.Members?.Length);
        var minimalMembers = eventModel.MinTeamMembers * 2;

        if (totalMembers < minimalMembers)
            return (false, "Для того, чтобы начать событие необходимо набрать минимальное количество участников");

        return (true, string.Empty);
    }

}
