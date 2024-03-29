using FluentAssertions;
using Hackathon.IntegrationServices.Github.Abstraction;
using Hackathon.IntegrationServices.Github.Services;
using System.Net.Http;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;

namespace Hackathon.BL.Tests.IntegrationServices;

public class GitHubIntegrationServiceTests: BaseUnitTest
{
    private readonly IGitHubIntegrationService _service;
    private readonly Mock<IHttpClientFactory> _httpClientFactoryMock = new();

    private const string ValidLink = "https://github.com/coder1coder/Hackathon/tree/develop";

    public GitHubIntegrationServiceTests()
    {
        _service = new GitHubIntegrationService(NullLogger<GitIntegrationService>.Instance, _httpClientFactoryMock.Object);
    }

    [Fact]
    public void ParseFromLink_IsFull_ShouldBe_True()
    {
        var parameters = _service.ParseFromLink(ValidLink);

        Assert.NotNull(parameters);
        parameters.IsFull.Should().BeTrue();
    }

    [Fact]
    public void ParseFromLink_ShouldParse()
    {
        var parameters = _service.ParseFromLink(ValidLink);

        Assert.NotNull(parameters);

        parameters.Url.Should().Be("https://github.com/");
        parameters.UserName.Should().Be("coder1coder");
        parameters.Repository.Should().Be("Hackathon");
        parameters.Branch.Should().Be("develop");
    }
}
