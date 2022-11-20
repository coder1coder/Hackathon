using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Hackathon.Common.Models.Event;
using Hackathon.Common.Models.Team;
using Hackathon.Contracts.Requests.Event;
using Hackathon.Contracts.Requests.Project;
using Hackathon.Contracts.Requests.Team;
using Hackathon.Tests.Integration.Base;
using Xunit;

namespace Hackathon.Tests.Integration.Project
{
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

            var user = await RegisterUser();
            SetToken(user.Token);

            var teamCreateResponse = await TeamsApi.Create(new CreateTeamRequest
            {
                Name = Guid.NewGuid().ToString()[..4],
                EventId = createEventResponse.Id,
                Type = (int)TeamType.Private
            });

            Assert.NotNull(teamCreateResponse);

            var createProjectResponse = await ProjectsApi.Create(new ProjectCreateRequest
            {
                TeamId = teamCreateResponse.Id,
                Name = Guid.NewGuid().ToString()[..8],
                Description = Guid.NewGuid().ToString(),
                EventId = createEventResponse.Id
            });

            Assert.NotNull(createProjectResponse);

            createProjectResponse.Id.Should().BeGreaterThan(0);
        }

    }
}
