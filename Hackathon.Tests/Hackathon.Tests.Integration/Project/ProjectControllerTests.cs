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
        //arrange
        var eventModel = TestFaker.GetEventModels(1, TestUser.Id).First();
        var eventRequest = Mapper.Map<CreateEventRequest>(eventModel);
        var createEventResponse = await EventsApi.Create(eventRequest);

        await EventsApi.SetStatus(new SetStatusRequest<EventStatus>
        {
            Id = createEventResponse.Id,
            Status = EventStatus.Published
        });

        var teamCreateResponse = await TeamApiClient.CreateAsync(new CreateTeamRequest
        {
            Name = Guid.NewGuid().ToString()[..4],
            EventId = createEventResponse.Id,
            Type = (int)TeamType.Private
        });

        Assert.NotNull(teamCreateResponse.Content);

        var user = await RegisterUser();
        SetToken(user.Token);

        //act
        await ProjectsApiClient.CreateAsync(new ProjectCreateRequest
        {
            TeamId = teamCreateResponse.Content.Id,
            Name = Guid.NewGuid().ToString()[..8],
            Description = Guid.NewGuid().ToString(),
            EventId = createEventResponse.Id
        });

        //assert
        var project = await ProjectsApiClient.GetAsync(createEventResponse.Id, teamCreateResponse.Content.Id);
        Assert.NotNull(project);
    }

    [Fact]
    public async Task Update_Should_Succeed()
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

        var teamCreateResponse = await TeamApiClient.CreateAsync(new CreateTeamRequest
        {
            Name = Guid.NewGuid().ToString()[..4],
            EventId = createEventResponse.Id,
            Type = (int)TeamType.Private
        });

        Assert.NotNull(teamCreateResponse.Content);

        var user = await RegisterUser();
        SetToken(user.Token);

        var createParameters = new ProjectCreateRequest
        {
            TeamId = teamCreateResponse.Content.Id,
            Name = Guid.NewGuid().ToString()[..8],
            Description = Guid.NewGuid().ToString(),
            EventId = createEventResponse.Id
        };

        await ProjectsApiClient.CreateAsync(createParameters);

        //act
        var updateParameters = new ProjectUpdateParameters
        {
            EventId = createEventResponse.Id,
            TeamId = teamCreateResponse.Content.Id,
            Description = Guid.NewGuid().ToString(),
            Name = Guid.NewGuid().ToString(),
        };

        await ProjectsApiClient.UpdateAsync(updateParameters);

        //assert
        var project = await ProjectsApiClient.GetAsync(createEventResponse.Id, teamCreateResponse.Content.Id);

        project.Name.Should().Be(updateParameters.Name);
        project.Description.Should().Be(updateParameters.Description);
    }

    [Fact]
    public async Task UpdateFromGitBranch_Should_Success()
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

        var teamCreateResponse = await TeamApiClient.CreateAsync(new CreateTeamRequest
        {
            Name = Guid.NewGuid().ToString()[..4],
            EventId = createEventResponse.Id,
            Type = (int)TeamType.Private
        });

        Assert.NotNull(teamCreateResponse.Content);

        var user = await RegisterUser();
        SetToken(user.Token);

        var createParameters = new ProjectCreateRequest
        {
            TeamId = teamCreateResponse.Content.Id,
            Name = Guid.NewGuid().ToString()[..8],
            Description = Guid.NewGuid().ToString(),
            EventId = createEventResponse.Id
        };

        await ProjectsApiClient.CreateAsync(createParameters);

        //act
        var request = new UpdateProjectFromGitBranchRequest
        {
            EventId = createEventResponse.Id,
            TeamId = teamCreateResponse.Content.Id,
            LinkToGitBranch = "https://github.com/coder1coder/Backend.Tools/tree/develop"
        };

        await ProjectsApiClient.UpdateProjectFromGitBranch(request);

        //assert
        var project = await ProjectsApiClient.GetAsync(request.EventId, request.TeamId);

        project.LinkToGitBranch.Should().Be(request.LinkToGitBranch);
        project.FileIds.Should().NotBeNullOrEmpty();
    }

}
