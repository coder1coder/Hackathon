namespace Hackathon.Common.ErrorMessages;

public static class EventMessages
{
    public const string EventNotFound = "Событие не существует";
    public const string CantAttachToEvent = "Нельзя присоединиться к событию";
    public const string AgreementShouldBeSigned = "Необходимо принять правила участия в мероприятии";
    public const string CantLeaveEventIfYouEnteredAsTeam = "Нельзя покинуть событие, если вступили командой";
    public const string UserIsNotInEvent = "Пользователь не состоит в событии";
    public const string CantLeaveEventWhenItsAlreadyStarted = "Нельзя покидать событие, когда оно уже начато";
    public const string CantSetEventStatus =  "Невозможно сменить статус события";
    public const string EventFileImageIsNotBeEmpty = "Файл не может быть пустым";
    public const string CantDeleteEventWithStatusOtherThаnDraftOnModeration = "Нельзя удалить событие со статусом отличным от статуса - Черновик или На модерации";
    public const string CantDeleteEventUserWhoNotOwner = "Нельзя удалить событие кому-то помимо организатора";
}
