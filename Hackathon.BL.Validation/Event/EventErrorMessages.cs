namespace Hackathon.BL.Validation.Event;

public static class EventErrorMessages
{
    public const string EventDoesNotExists = "Событие не существует";
    public const string AgreementDoesNotExists = "Соглашение об участии в мероприятии не существует";
    public const string EventSavedButImageDoesNotExists
        = "Событие сохранено, но картинка мероприятия не существует. Загрузите картинку еще раз";

    public const string NoRightsExecuteOperation = "Нет прав на выполнение операции";
    public const string IncorrectStatusForUpdating = "Нельзя редактировать мероприятие в текущем статусе";
}
