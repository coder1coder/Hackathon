using FluentAssertions;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
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

public class TeamApiTests: BaseIntegrationTest
{
    public TeamApiTests(TestWebApplicationFactory factory) : base(factory)
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

        var teamCreateResponse = await TeamsClient.Create(new CreateTeamRequest
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
        var teamCreateResponse = await TeamsClient.Create(new CreateTeamRequest
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

        await TeamsClient.Create(new CreateTeamRequest
        {
            Name = Guid.NewGuid().ToString()[..4],
            Type = TeamType.Private
        });

        var (_, secondUserToken) = await RegisterUser();
        SetToken(secondUserToken);

       await TeamsClient.Create(new CreateTeamRequest
       {
           Name = Guid.NewGuid().ToString()[..4],
           Type = TeamType.Public
       });

        //act
        var getTeamListResponse = await TeamsClient.GetListAsync(new GetListParameters<TeamFilter>
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

    [Fact]
    public async Task GetSentJoinRequest_Should_Success()
    {
        //arrange
        var teamOwner = await RegisterUser();
        SetToken(teamOwner.Token);

        var teamCreateResponse = await TeamsClient.Create(new CreateTeamRequest
        {
            Name = Guid.NewGuid().ToString()[..4],
            Type = TeamType.Private
        });

        var teamId = teamCreateResponse.Content?.Id ?? default;

        var user = await RegisterUser();
        SetToken(user.Token);

        await TeamsClient.CreateJoinRequestAsync(teamId);

        //act
        var response = await TeamsClient.GetSentJoinRequestAsync(teamId);

        //assert
        Assert.True(response.IsSuccessStatusCode);
        Assert.NotNull(response.Content);
        response.Content.Status.Should().Be(TeamJoinRequestStatus.Sent);
    }

    [Fact]
    public async Task CancelJoinRequest_Should_Success()
    {
        //arrange
        var teamOwner = await RegisterUser();
        SetToken(teamOwner.Token);

        var teamCreateResponse = await TeamsClient.Create(new CreateTeamRequest
        {
            Name = Guid.NewGuid().ToString()[..4],
            Type = TeamType.Private
        });

        var teamId = teamCreateResponse.Content?.Id ?? default;

        var user = await RegisterUser();
        SetToken(user.Token);

        var createResponse = await TeamsClient.CreateJoinRequestAsync(teamId);


        //act
        await TeamsClient.CancelJoinRequestAsync(new CancelRequestParameters
        {
            RequestId = createResponse.Id
        });
        var response = await TeamsClient.GetSentJoinRequestAsync(teamId);

        //assert
        Assert.False(response.IsSuccessStatusCode);
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetTeamSentJoinRequestsAsync_Should_Success()
    {
        //arrange
        var (_, teamOwnerToken) = await RegisterUser();
        SetToken(teamOwnerToken);

        var teamCreateResponse = await TeamsClient.Create(new CreateTeamRequest
        {
            Name = Guid.NewGuid().ToString()[..4],
            Type = TeamType.Private
        });

        var teamId = teamCreateResponse.Content?.Id ?? default;

        const int joinRequestCount = 5;

        for (var i = 0; i < joinRequestCount; i++)
        {
            var createdUser = await RegisterUser();
            SetToken(createdUser.Token);
            await TeamsClient.CreateJoinRequestAsync(teamId);
        }

        //act
        SetToken(teamOwnerToken);

        var response = await TeamsClient.GetTeamSentJoinRequestsAsync(teamId, new PaginationSort
        {
            Offset = 0,
            Limit = joinRequestCount
        });

        //assert
        Assert.True(response.IsSuccessStatusCode);
        Assert.NotNull(response.Content);

        response.Content.TotalCount.Should().Be(joinRequestCount);
        response.Content.Items.Count.Should().Be(joinRequestCount);
    }

    [Fact]
    public async Task GetTeamSentJoinRequestsAsync_Should_Forbidden_When_User_IsNot_Owner()
    {
        //arrange
        var (_, teamOwnerToken) = await RegisterUser();
        SetToken(teamOwnerToken);

        var teamCreateResponse = await TeamsClient.Create(new CreateTeamRequest
        {
            Name = Guid.NewGuid().ToString()[..4],
            Type = TeamType.Private
        });

        var teamId = teamCreateResponse.Content?.Id ?? default;

        var createdUser = await RegisterUser();
        SetToken(createdUser.Token);

        await TeamsClient.CreateJoinRequestAsync(teamId);

        //act
        var response = await TeamsClient.GetTeamSentJoinRequestsAsync(teamId, new PaginationSort
        {
            Offset = 0,
            Limit = 1
        });

        //assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task ApproveJoinRequest_Should_AddMemberToTeam()
    {
        //arrange
        var (_, teamOwnerToken) = await RegisterUser();
        SetToken(teamOwnerToken);

        var teamCreateResponse = await TeamsClient.Create(new CreateTeamRequest
        {
            Name = Guid.NewGuid().ToString()[..4],
            Type = TeamType.Private
        });

        var teamId = teamCreateResponse.Content?.Id ?? default;

        var (userId, userToken) = await RegisterUser();
        SetToken(userToken);

        var createResponse = await TeamsClient.CreateJoinRequestAsync(teamId);

        //act
        SetToken(teamOwnerToken);
        await TeamsClient.ApproveJoinRequest(createResponse.Id);

        var response = await TeamsClient.Get(teamId);

        //assert
        response.HasMember(userId).Should().BeTrue();
    }

    [Fact]
    public async Task ApproveJoinRequest_Should_UpdateRequestStatus()
    {
        //arrange
        var (_, teamOwnerToken) = await RegisterUser();
        SetToken(teamOwnerToken);

        var teamCreateResponse = await TeamsClient.Create(new CreateTeamRequest
        {
            Name = Guid.NewGuid().ToString()[..4],
            Type = TeamType.Private
        });

        var teamId = teamCreateResponse.Content?.Id ?? default;

        var (userId, userToken) = await RegisterUser();
        SetToken(userToken);

        var createResponse = await TeamsClient.CreateJoinRequestAsync(teamId);

        //act
        SetToken(teamOwnerToken);
        await TeamsClient.ApproveJoinRequest(createResponse.Id);

        SetToken(userToken);
        var response = await TeamsClient.GetJoinRequests(new GetListParameters<TeamJoinRequestFilter>
        {
            Filter = new TeamJoinRequestFilter
            {
                TeamId = teamId
            }
        });

        //assert
        Assert.True(response.IsSuccessStatusCode);
        Assert.NotNull(response.Content?.Items);

        Assert.Contains(response.Content.Items, x =>
            x.Id == createResponse.Id
            && x.TeamId == teamId
            && x.UserId == userId
            && x.Status == TeamJoinRequestStatus.Accepted);
    }
}
