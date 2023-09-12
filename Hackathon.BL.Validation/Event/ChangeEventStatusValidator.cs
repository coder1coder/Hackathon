using Hackathon.Common.ErrorMessages;
using Hackathon.Common.Models.Event;
using Hackathon.Common.Models.User;

namespace Hackathon.BL.Validation.Event;

public static class ChangeEventStatusValidator
{
    public static (bool, string) ValidateAsync(UserRole userRole, EventModel eventModel, EventStatus newStatus)
    {
        var eventStatusOrdered = new[]
        {
            EventStatus.Draft,
            EventStatus.OnModeration,
            EventStatus.Published,
            EventStatus.Started,
            EventStatus.Finished
        };

        var currentStatusIndex = Array.IndexOf(eventStatusOrdered, eventModel.Status);
        var newStatusIndex = Array.IndexOf(eventStatusOrdered, newStatus);

        var canChangeStatus =
            currentStatusIndex >= 0 && newStatusIndex >= 0
            &&
            (
                //пользователь завершает мероприятие
                newStatus == EventStatus.Finished
                ||
                //новый статус является следующим в последовательности
                currentStatusIndex == newStatusIndex - 1
            );

        if (!canChangeStatus)
            return (false, EventMessages.CantSetEventStatus);

        return newStatus switch
        {
            EventStatus.Draft => ValidateDraft(eventModel.Status),
            EventStatus.OnModeration => ValidateOnModeration(eventModel.Status),
            EventStatus.Published => ValidatePublished(userRole),
            EventStatus.Started => CanBeStarted(eventModel),

            _ => (true, string.Empty)
        };
    }

    private static (bool, string) ValidateDraft(EventStatus eventStatus)
        => eventStatus == EventStatus.OnModeration
            ? (true, string.Empty)
            : (false, "Только мероприятия находящиеся на модерации или не прошедшие её могут быть переведены в статус Черновик");

    private static (bool, string) ValidateOnModeration(EventStatus currentEventStatus)
        => currentEventStatus == EventStatus.Draft
            ? (true, string.Empty)
            : (false, "Отправить на модерацию можно только мероприятие в статусе Черновик");

    private static (bool, string) ValidatePublished(UserRole userRole)
        => userRole == UserRole.Administrator
            ? (true, string.Empty)
            : (false, "Мероприятие должно пройти модерацию прежде чем будет опубликовано");

    private static (bool, string) CanBeStarted(EventModel eventModel)
    {
        var totalMembers = eventModel.Teams.Sum(x => x.Members?.Length);
        var minimalMembers = eventModel.MinTeamMembers * 2;

        return totalMembers < minimalMembers ?
            (false, "Для того, чтобы начать событие необходимо набрать минимальное количество участников")
            : (true, string.Empty);
    }

}
