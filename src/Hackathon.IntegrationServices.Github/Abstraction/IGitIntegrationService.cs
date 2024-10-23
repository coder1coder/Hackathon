namespace Hackathon.IntegrationServices.Github.Abstraction;

public interface IGitIntegrationService
{
    /// <summary>
    /// Получить список наименований репозиториев
    /// </summary>
    Task<string[]> GetRepositoryNames();

    /// <summary>
    /// Получить список веток
    /// </summary>
    Task<string[]> GetBranchNames();

    /// <summary>
    /// Получить содержимое из репозитория
    /// </summary>
    Task<Stream> ReceiveFromRepository(GitParameters parameters);

    /// <summary>
    /// Получить параметры по ссылке
    /// </summary>
    /// <param name="link"></param>
    GitParameters ParseFromLink(string link);
}
