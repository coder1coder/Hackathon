using System;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using Hackathon.Common.Models.Event;
using Hackathon.Contracts.Requests.Event;
using Xunit;

namespace Hackathon.Tests.Integration.Event
{
    public class EventControllerTests : BaseIntegrationTest
    {
        public EventControllerTests(TestWebApplicationFactory factory)
            : base(factory)
        {
        }

        [Fact]
        public async Task Get_ShouldReturn_Success()
        {
            var eventModel = TestFaker.GetEventModels(1, TestUser.Id).First();
            var createEventModel = Mapper.Map<CreateEventRequest>(eventModel);

            var baseCreateResponse = await EventsApi.Create(createEventModel);
            var getEventResponse = await EventsApi.Get(baseCreateResponse.Id);
            eventModel = getEventResponse.Content;

            Assert.NotNull(eventModel);

            eventModel.Id.Should().Be(baseCreateResponse.Id);

            eventModel.Should().BeEquivalentTo(createEventModel, options =>
                options
                    .Using<DateTime>(x =>
                        x.Subject.Should().BeCloseTo(x.Expectation, TimeSpan.FromMilliseconds(1)))
                    .WhenTypeIs<DateTime>());
        }

        [Fact]
        public async Task SetStatus_FromDraft_ToPublished_ShouldReturn_Success()
        {
            var eventModel = TestFaker.GetEventModels(1, TestUser.Id).First();
            var createEventRequest = Mapper.Map<CreateEventRequest>(eventModel);

            var createEventResponse = await EventsApi.Create(createEventRequest);

            await FluentActions
                .Invoking(async () => await EventsApi.SetStatus(new SetStatusRequest<EventStatus>
                {
                    Id = createEventResponse.Id,
                    Status = EventStatus.Published
                }))
                .Should()
                .NotThrowAsync();

            var getEventResponse = await EventsApi.Get(createEventResponse.Id);
            eventModel = getEventResponse.Content;

            Assert.NotNull(eventModel);

            eventModel.Status.Should().Be(EventStatus.Published);
        }

        [Fact]
        public async Task StartEvent_ShouldSuccess()
        {
            // Создаем событие
            var request = new Faker<CreateEventRequest>()
                .RuleFor(x => x.Description, f => f.Random.String2(400))
                .RuleFor(x => x.Name, Guid.NewGuid().ToString())
                .RuleFor(x => x.Start, DateTime.UtcNow.AddDays(1))
                .RuleFor(x => x.DevelopmentMinutes, 10)
                .RuleFor(x => x.TeamPresentationMinutes, 10)
                .RuleFor(x => x.MemberRegistrationMinutes, 10)
                .RuleFor(x => x.IsCreateTeamsAutomatically, true)
                .RuleFor(x => x.MinTeamMembers, 1)
                .RuleFor(x => x.MaxEventMembers, 2)
                .RuleFor(x => x.Award, "0")
                .Generate(1)
                .First();

            var createEventResponse = await EventsApi.Create(request);

            // Публикуем событие, чтобы можно было регистрироваться участникам
            await EventsApi.SetStatus(new SetStatusRequest<EventStatus>
            {
                Id = createEventResponse.Id,
                Status = EventStatus.Published
            });

            // Присоединяемся к событию в качестве участника
            await EventsApi.Join(createEventResponse.Id);

            // Регистрируем нового участника в событии
            var user = await RegisterUser();
            SetToken(user.Token);
            await EventsApi.Join(createEventResponse.Id);

            // Начинаем событие
            SetToken(TestUser.Token);

            await EventsApi.SetStatus(new SetStatusRequest<EventStatus>
            {
                Id = createEventResponse.Id,
                Status = EventStatus.Started
            });
        }
    }
}
