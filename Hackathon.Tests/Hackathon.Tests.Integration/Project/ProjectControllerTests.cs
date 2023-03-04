using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Hackathon.Common.Models.Event;
using Hackathon.Common.Models.Project;
using Hackathon.Common.Models.Team;
using Hackathon.Contracts.Requests.Event;
using Hackathon.Contracts.Requests.Project;
using Hackathon.Contracts.Requests.Team;
using Xunit;

namespace Hackathon.Tests.Integration.Project;

public class ProjectControllerTests : BaseIntegrationTest
{
    public ProjectControllerTests(TestWebApplicationFactory factory) : base(factory)
    {

    }

    [Fact]
    public async Task Create_Should_Success()
    {
        var eventModel = TestFaker.GetEventModels(1, TestUser.Id).First();
        var eventRequest = Mapper.Map<CreateEventRequest>(eventModel);
        var createEventResponse = await EventsApi.Create(eventRequest);

        await EventsApi.SetStatus(new SetStatusRequest<EventStatus>
        {
            Id = createEventResponse.Id,
            Status = EventStatus.Published
        });

        var teamCreateResponse = await TeamsApi.Create(new CreateTeamRequest
        {
            Name = Guid.NewGuid().ToString()[..4],
            EventId = createEventResponse.Id,
            Type = (int)TeamType.Private
        });

        Assert.NotNull(teamCreateResponse?.Content);

        var user = await RegisterUser();
        SetToken(user.Token);

        var createProjectResponse = await ProjectsApi.Create(new ProjectCreateRequest
        {
            TeamId = teamCreateResponse.Content.Id,
            Name = Guid.NewGuid().ToString()[..8],
            Description = Guid.NewGuid().ToString(),
            EventId = createEventResponse.Id
        });

        Assert.NotNull(createProjectResponse);

        createProjectResponse.Id.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task UpdateFromGit_ShouldBe_Succeed()
    {
        //arrange
        var eventModel = TestFaker.GetEventModels(1, TestUser.Id).First();
        var eventRequest = Mapper.Map<CreateEventRequest>(eventModel);
        var createEventResponse = await EventsApi.Create(eventRequest);

        await EventsApi.SetStatus(new SetStatusRequest<EventStatus>
        {
            Id = createEventResponse.Id,
            Status = EventStatus.Published
        });

        var teamCreateResponse = await TeamsApi.Create(new CreateTeamRequest
        {
            Name = Guid.NewGuid().ToString()[..4],
            EventId = createEventResponse.Id,
            Type = (int)TeamType.Private
        });

        Assert.NotNull(teamCreateResponse?.Content);

        var user = await RegisterUser();
        SetToken(user.Token);

        var createProjectResponse = await ProjectsApi.Create(new ProjectCreateRequest
        {
            TeamId = teamCreateResponse.Content.Id,
            Name = Guid.NewGuid().ToString()[..8],
            Description = Guid.NewGuid().ToString(),
            EventId = createEventResponse.Id
        });

        var parameters = new ProjectUpdateFromGitParameters
        {
            ProjectId = createProjectResponse.Id,
            LinkToGitBranch = "https://github.com/coder1coder/Backend.Tools/tree/develop"
        };

        //act
        await ProjectsApi.UpdateFromGit(parameters);

        //assert
        var project = await ProjectsApi.Get(parameters.ProjectId);

        project.FileIds.Should().NotBeNullOrEmpty();
    }

}
