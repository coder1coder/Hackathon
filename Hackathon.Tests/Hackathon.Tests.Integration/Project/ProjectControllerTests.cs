using FluentAssertions;
using Hackathon.Common.Models.Event;
using Hackathon.Common.Models.Project;
using Hackathon.Contracts.Requests.Event;
using Hackathon.Contracts.Requests.Project;
using System;
using System.Linq;
using System.Threading.Tasks;
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
        var @event = TestFaker.GetEventModels(1, TestUser.Id, EventStatus.Draft).First();
        var request = Mapper.Map<CreateEventRequest>(@event);
        var createEventResponse = await EventsApi.Create(request);

        // Публикуем событие, чтобы можно было регистрироваться участникам
        await EventsApi.SetStatus(new SetStatusRequest<EventStatus>
        {
            Id = createEventResponse.Id,
            Status = EventStatus.Published
        });

        // Присоединяемся к событию в качестве участника
        await EventsApi.JoinAsync(createEventResponse.Id);

        // Регистрируем нового участника в событии
        var user = await RegisterUser();
        SetToken(user.Token);
        await EventsApi.JoinAsync(createEventResponse.Id);

        // Начинаем событие
        SetToken(TestUser.Token);

        await EventsApi.SetStatus(new SetStatusRequest<EventStatus>
        {
            Id = createEventResponse.Id,
            Status = EventStatus.Started
        });

        var getEventResponse = await EventsApi.Get(createEventResponse.Id);
        var temporaryTeamByUserId = getEventResponse.Content?.Teams?.FirstOrDefault(x => x.HasMemberWithId(TestUser.Id));

        //act
        await ProjectsApiClient.CreateAsync(new ProjectCreateRequest
        {
            TeamId = temporaryTeamByUserId?.Id ?? default,
            Name = Guid.NewGuid().ToString()[..8],
            Description = Guid.NewGuid().ToString(),
            EventId = createEventResponse.Id
        });

        //assert
        var project = await ProjectsApiClient.GetAsync(createEventResponse.Id, temporaryTeamByUserId?.Id ?? default);
        Assert.NotNull(project);
    }

    [Fact]
    public async Task Update_Should_Succeed()
    {
        //arrange
        var @event = TestFaker.GetEventModels(1, TestUser.Id, EventStatus.Draft).First();
        var request = Mapper.Map<CreateEventRequest>(@event);
        var createEventResponse = await EventsApi.Create(request);

        // Публикуем событие, чтобы можно было регистрироваться участникам
        await EventsApi.SetStatus(new SetStatusRequest<EventStatus>
        {
            Id = createEventResponse.Id,
            Status = EventStatus.Published
        });

        // Присоединяемся к событию в качестве участника
        await EventsApi.JoinAsync(createEventResponse.Id);

        // Регистрируем нового участника в событии
        var user = await RegisterUser();
        SetToken(user.Token);
        await EventsApi.JoinAsync(createEventResponse.Id);

        // Начинаем событие
        SetToken(TestUser.Token);

        await EventsApi.SetStatus(new SetStatusRequest<EventStatus>
        {
            Id = createEventResponse.Id,
            Status = EventStatus.Started
        });

        var getEventResponse = await EventsApi.Get(createEventResponse.Id);
        var temporaryTeamByUserId = getEventResponse.Content?.Teams?.FirstOrDefault(x => x.HasMemberWithId(TestUser.Id));

        var createParameters = new ProjectCreateRequest
        {
            TeamId = temporaryTeamByUserId?.Id ?? default,
            Name = Guid.NewGuid().ToString()[..8],
            Description = Guid.NewGuid().ToString(),
            EventId = createEventResponse.Id
        };

        await ProjectsApiClient.CreateAsync(createParameters);

        //act
        var updateParameters = new ProjectUpdateParameters
        {
            EventId = createEventResponse.Id,
            TeamId = temporaryTeamByUserId?.Id ?? default,
            Description = Guid.NewGuid().ToString(),
            Name = Guid.NewGuid().ToString(),
        };

        await ProjectsApiClient.UpdateAsync(updateParameters);

        //assert
        var project = await ProjectsApiClient.GetAsync(createEventResponse.Id, temporaryTeamByUserId?.Id ?? default);

        project.Name.Should().Be(updateParameters.Name);
        project.Description.Should().Be(updateParameters.Description);
    }

    [Fact]
    public async Task UpdateFromGitBranch_Should_Success()
    {
        //arrange
        var @event = TestFaker.GetEventModels(1, TestUser.Id, EventStatus.Draft).First();
        var request = Mapper.Map<CreateEventRequest>(@event);
        var createEventResponse = await EventsApi.Create(request);

        // Публикуем событие, чтобы можно было регистрироваться участникам
        await EventsApi.SetStatus(new SetStatusRequest<EventStatus>
        {
            Id = createEventResponse.Id,
            Status = EventStatus.Published
        });

        // Присоединяемся к событию в качестве участника
        await EventsApi.JoinAsync(createEventResponse.Id);

        // Регистрируем нового участника в событии
        var user = await RegisterUser();
        SetToken(user.Token);
        await EventsApi.JoinAsync(createEventResponse.Id);

        // Начинаем событие
        SetToken(TestUser.Token);

        await EventsApi.SetStatus(new SetStatusRequest<EventStatus>
        {
            Id = createEventResponse.Id,
            Status = EventStatus.Started
        });

        var getEventResponse = await EventsApi.Get(createEventResponse.Id);
        var temporaryTeamByUserId = getEventResponse.Content?.Teams?.FirstOrDefault(x => x.HasMemberWithId(TestUser.Id));

        var createParameters = new ProjectCreateRequest
        {
            TeamId = temporaryTeamByUserId?.Id ?? default,
            Name = Guid.NewGuid().ToString()[..8],
            Description = Guid.NewGuid().ToString(),
            EventId = createEventResponse.Id
        };

        await ProjectsApiClient.CreateAsync(createParameters);

        //act
        var updateProjectRequest = new UpdateProjectFromGitBranchRequest
        {
            EventId = createEventResponse.Id,
            TeamId = temporaryTeamByUserId?.Id ?? default,
            LinkToGitBranch = "https://github.com/coder1coder/Backend.Tools/tree/develop"
        };

        await ProjectsApiClient.UpdateProjectFromGitBranch(updateProjectRequest);

        //assert
        var project = await ProjectsApiClient.GetAsync(updateProjectRequest.EventId, updateProjectRequest.TeamId);

        project.LinkToGitBranch.Should().Be(updateProjectRequest.LinkToGitBranch);
        project.FileIds.Should().NotBeNullOrEmpty();
    }

}
