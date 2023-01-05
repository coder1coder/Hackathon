using System;
using System.IO;
using System.Threading.Tasks;
using Hackathon.Abstraction.IntegrationServices;

namespace Hackathon.IntegrationServices.Services;

public abstract class GitIntegrationService: IGitIntegrationService
{
    public virtual Task<string[]> GetRepositoryNames()
        => Task.FromResult(Array.Empty<string>());

    public virtual Task<string[]> GetBranchNames()
        => Task.FromResult(Array.Empty<string>());

    public abstract Task<Stream> ReceiveFromRepository(GitParameters parameters);

    public abstract GitParameters ParseFromLink(string link);
}
