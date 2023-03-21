using FluentAssertions;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Event;
using Hackathon.Common.Models.Team;
using Hackathon.Contracts.Requests.Event;
using Hackathon.Contracts.Requests.Team;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Hackathon.Tests.Integration.Team;

public class TeamControllerTests: BaseIntegrationTest
{
    public TeamControllerTests(TestWebApplicationFactory factory) : base(factory)
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
            EventId = createEventResponse.Id
        });

        Assert.True(teamCreateResponse.IsSuccessStatusCode);
        Assert.NotNull(teamCreateResponse.Content);
        teamCreateResponse.Content.Id.Should().BeGreaterThan(0);
        var teamExist = await TeamRepository.ExistAsync(teamCreateResponse.Content.Id);

        teamExist.Should().BeTrue();
    }

    [Fact]
    public async Task Create_When_EventOwner_IsNot_AuthUser_Should_Fail()
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

        var user = await RegisterUser();
        SetToken(user.Token);

        //act
        var teamCreateResponse = await TeamsApi.Create(new CreateTeamRequest
        {
            Name = Guid.NewGuid().ToString()[..4],
            EventId = createEventResponse.Id
        });

        //assert
        teamCreateResponse.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task GetList_Should_Return_Only_Public_Teams()
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

        await TeamsApi.Create(new CreateTeamRequest
        {
            Name = Guid.NewGuid().ToString()[..4],
            Type = TeamType.Private
        });

        var (_, secondUserToken) = await RegisterUser();
        SetToken(secondUserToken);

       await TeamsApi.Create(new CreateTeamRequest
        {
            Name = Guid.NewGuid().ToString()[..4],
            Type = TeamType.Public
        });

        //act
        var getTeamListResponse = await TeamsApi.GetListAsync(new GetListParameters<TeamFilter>
        {
            Filter = new TeamFilter
            {
                TeamType = TeamType.Public
            }
        });

        //assert
        Assert.True(getTeamListResponse.Content?.Items is {Count: > 0});
        Assert.True(getTeamListResponse.Content.Items.All(x => x.Type == TeamType.Public));
    }
}
