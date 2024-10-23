using Hackathon.IntegrationServices.Github.Abstraction;

namespace Hackathon.IntegrationServices.Github.Services;

public abstract class GitIntegrationService: IGitIntegrationService
{
    public abstract Task<string[]> GetRepositoryNames();

    public abstract Task<string[]> GetBranchNames();

    public abstract Task<Stream> ReceiveFromRepository(GitParameters parameters);

    public abstract GitParameters ParseFromLink(string link);
}
