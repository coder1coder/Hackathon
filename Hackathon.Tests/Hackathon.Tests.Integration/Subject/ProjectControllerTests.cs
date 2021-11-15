using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Hackathon.Common.Models.Event;
using Hackathon.Contracts.Requests.Event;
using Hackathon.Contracts.Requests.Project;
using Hackathon.Contracts.Requests.Team;
using Hackathon.Tests.Integration.Base;
using Mapster;
using Xunit;

namespace Hackathon.Tests.Integration.Subject
{
    public class ProjectControllerTests: BaseIntegrationTest, IClassFixture<TestWebApplicationFactory>
    {
        public ProjectControllerTests(TestWebApplicationFactory factory) : base(factory)
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

            var createProjectResponse = await ApiService.Projects.CreateAsync(new ProjectCreateRequest
            {
                TeamId = teamCreateResponse.Id,
                Name = Guid.NewGuid().ToString()[..8],
                Description = Guid.NewGuid().ToString()
            });

            Assert.NotNull(createProjectResponse);

            createProjectResponse.Id.Should().BeGreaterThan(0);
        }

    }
}