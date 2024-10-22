namespace Hackathon.BL.Validation.Projects;

public static class ProjectValidationErrorMessages
{
    public const string ProjectDoesNotExist = "Проект не существует";
    public const string ProjectAlreadyExists = "Проект уже существует";
    public const string IncorrectGitBranch = "Указана некорректная ссылка на GIT репозиторий";
    public const string ProjectCanBeCreatedOnlyForStartedEvent = "Проект может быть добавлен только для мероприятия, которое начато";
}
