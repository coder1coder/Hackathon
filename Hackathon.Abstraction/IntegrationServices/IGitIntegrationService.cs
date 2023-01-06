namespace Hackathon.Abstraction.IntegrationServices;

public interface IGitIntegrationService
{
    /// <summary>
    /// Получить список наименований репозиториев
    /// </summary>
    /// <returns></returns>
    Task<string[]> GetRepositoryNames();

    /// <summary>
    /// Получить список веток
    /// </summary>
    /// <returns></returns>
    Task<string[]> GetBranchNames();

    /// <summary>
    /// Получить содержимое из репозитория
    /// </summary>
    /// <returns></returns>
    Task<Stream> ReceiveFromRepository(GitParameters parameters);

    /// <summary>
    /// Получить параметры по ссылке
    /// </summary>
    /// <param name="link"></param>
    /// <returns></returns>
    GitParameters ParseFromLink(string link);
}
