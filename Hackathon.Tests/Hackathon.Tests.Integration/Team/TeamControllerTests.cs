using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Hackathon.Common.Models.Event;
using Hackathon.Contracts.Requests.Event;
using Hackathon.Contracts.Requests.Team;
using Hackathon.Contracts.Requests.User;
using Hackathon.Tests.Integration.Base;
using Xunit;

namespace Hackathon.Tests.Integration.Team
{
    public class TeamControllerTests : BaseIntegrationTest, IClassFixture<TestWebApplicationFactory>
    {
        public TeamControllerTests(TestWebApplicationFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task Create_Should_Success()
        {
            var eventModel = TestFaker.GetEventModels(1, UserId).First();
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

            Assert.NotNull(teamCreateResponse);
            teamCreateResponse.Id.Should().BeGreaterThan(0);

            var teamExist = await TeamRepository.ExistAsync(teamCreateResponse.Id);

            teamExist.Should().BeTrue();
        }

      /*  [Fact]
        public async Task AddMember_Should_Success()
        {
            var eventModel = TestFaker.GetEventModels(1, UserId).First();
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

            var fakeRequest = Mapper.Map<SignUpRequest>(TestFaker.GetSignUpModels(1).First());

            var createUserResponse = await UsersApi.SignUp(fakeRequest);

            await FluentActions.Invoking(async () =>
                {
                    await TeamsApi.AddMember(new TeamAddMemberRequest
                    {
                        TeamId = teamCreateResponse.Id,
                        UserId = createUserResponse.Id
                    });
                })
                .Should().NotThrowAsync<Exception>();
        }*/
    }
}