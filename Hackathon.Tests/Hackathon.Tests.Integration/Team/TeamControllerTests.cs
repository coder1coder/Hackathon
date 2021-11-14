using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Hackathon.Common.Models.Event;
using Hackathon.Contracts.Requests.Event;
using Hackathon.Contracts.Requests.Team;
using Hackathon.Tests.Integration.Base;
using Mapster;
using Xunit;

namespace Hackathon.Tests.Integration.Team
{
    public class TeamControllerTests: BaseIntegrationTest, IClassFixture<TestWebApplicationFactory>
    {
        public TeamControllerTests(TestWebApplicationFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task Create_Should_Success()
        {
            var eventModel = TestFaker.GetEventModels(1).First();
            var eventRequest = eventModel.Adapt<CreateEventRequest>();
            var createEventResponse = await ApiService.Events.Create(eventRequest);
            await ApiService.Events.SetStatus(new SetStatusRequest<EventStatus>
            {
                Id = createEventResponse.Id,
                Status = EventStatus.Published
            });

            var teamCreateResponse = await ApiService.Teams.Create(new CreateTeamRequest
            {
                Name = Guid.NewGuid().ToString()[..4],
                EventId = createEventResponse.Id
            });

            Assert.NotNull(teamCreateResponse);
            teamCreateResponse.Id.Should().BeGreaterThan(0);

            var teamExist = await TeamRepository.ExistAsync(teamCreateResponse.Id);

            teamExist.Should().BeTrue();
        }

    }
}