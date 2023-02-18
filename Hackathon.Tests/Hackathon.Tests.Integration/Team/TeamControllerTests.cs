using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Hackathon.BL.Team;
using Hackathon.Common.Models.Event;
using Hackathon.Contracts.Requests.Event;
using Hackathon.Contracts.Requests.Team;
using Refit;
using Xunit;

namespace Hackathon.Tests.Integration.Team
{
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

            var teamCreateResponse = await TeamsApi.Create(new CreateTeamRequest
            {
                Name = Guid.NewGuid().ToString()[..4],
                EventId = createEventResponse.Id
            });

            Assert.False(teamCreateResponse.IsSuccessStatusCode);
            Assert.IsType<ValidationApiException>(teamCreateResponse.Error);
            var exception = (ValidationApiException) teamCreateResponse.Error;

            Assert.NotNull(exception.Content);
            Assert.Equal(TeamService.CreateTeamAccessDenied, exception.Content.Detail);
        }
    }
}
