using Hackathon.Common;
using Hackathon.IntegrationServices.Github.Abstraction;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

namespace Hackathon.IntegrationServices.Github.Services;

public class GitHubIntegrationService: GitIntegrationService, IGitHubIntegrationService
{
    private readonly ILogger<GitIntegrationService> _logger;
    private readonly HttpClient _httpClient;

    public GitHubIntegrationService(ILogger<GitIntegrationService> logger, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClient = httpClientFactory.CreateClient();
    }

    public override Task<string[]> GetRepositoryNames()
    {
        throw new NotImplementedException();
    }

    public override Task<string[]> GetBranchNames()
    {
        throw new NotImplementedException();
    }

    public override async Task<Stream> ReceiveFromRepository(GitParameters parameters)
    {
        _logger.LogInformation("{Initiator}. Начато получение содержимого из репозитория: {@parameters}",
            nameof(GitHubIntegrationService),
            parameters);

        try
        {
            return await _httpClient.GetStreamAsync(parameters.ToZipLink());
        }
        catch (Exception e)
        {
            _logger.LogError(e, "{Source}. Ошибка во время получения содержимого репозитория: {error}",
                nameof(GitHubIntegrationService),
                e.Message);

            return null;
        }
    }

    public override GitParameters ParseFromLink(string link)
    {
        var parameters = Regex.Split(link, RegexPatterns.GithubBranchLink, RegexOptions.None)
            .Where(x => !string.IsNullOrEmpty(x))
            .ToArray();

        var gitParameters = new GitParameters();

        if (parameters.Length > 0)
        {
            gitParameters.Url = parameters[0];
        }

        if (parameters.Length > 1)
        {
            gitParameters.UserName = parameters[1];
        }

        if (parameters.Length > 2)
        {
            gitParameters.Repository = parameters[2];
        }

        if (parameters.Length > 3)
        {
            gitParameters.Branch = parameters[3];
        }

        return gitParameters;
    }
}
