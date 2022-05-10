using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Hackathon.Common.Models.Event;
using Hackathon.Contracts.Requests.Event;
using Hackathon.Tests.Integration.Base;
using Xunit;

namespace Hackathon.Tests.Integration.Event
{
    public class EventControllerTests: BaseIntegrationTest
    {
        public EventControllerTests(TestWebApplicationFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task Get_ShouldReturn_Success()
        {
            var eventModel = TestFaker.GetEventModels(1, TestUser.Id).First();
            var createEventModel = Mapper.Map<CreateEventModel>(eventModel);

            var eventId = await EventRepository.CreateAsync(createEventModel);
            eventModel = await EventsApi.Get(eventId);

            Assert.NotNull(eventModel);

            eventModel.Id.Should().Be(eventId);

            eventModel.Should().BeEquivalentTo(createEventModel, options =>
                options
                    .Using<DateTime>(x=>
                        x.Subject.Should().BeCloseTo(x.Expectation, TimeSpan.FromMilliseconds(1)
                        )).WhenTypeIs<DateTime>());
        }

        [Fact]
        public async Task SetStatus_FromDraft_ToPublished_ShouldReturn_Success()
        {
            var eventModel = TestFaker.GetEventModels(1, TestUser.Id).First();
            var createEventModel = Mapper.Map<CreateEventModel>(eventModel);

            var eventId = await EventRepository.CreateAsync(createEventModel);

            await FluentActions
                .Invoking(async () => await EventsApi.SetStatus(new SetStatusRequest<EventStatus>
                {
                    Id = eventId,
                    Status = EventStatus.Published
                }))
                .Should()
                .NotThrowAsync();

            eventModel = await EventsApi.Get(eventId);
            eventModel.Status.Should().Be(EventStatus.Published);
        }

        [Fact]
        public async Task StartEvent_ShouldSuccess()
        {
            //Создаем событие
            var createEventResponse = await EventsApi.Create(new CreateEventRequest
            {
                Name = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
                Start = DateTime.UtcNow.AddDays(1),
                DevelopmentMinutes = 10,
                TeamPresentationMinutes = 10,
                MemberRegistrationMinutes = 10,
                IsCreateTeamsAutomatically = true,
                MinTeamMembers = 1,
                MaxEventMembers = 2,
                Award = "0"
            });

            //Публикуем событие, чтобы можно было регистрироваться участникам
            await EventsApi.SetStatus(new SetStatusRequest<EventStatus>
            {
                Id = createEventResponse.Id,
                Status = EventStatus.Published
            });

            //Присоединяемся к событию в качестве участника
            await EventsApi.Join(createEventResponse.Id);

            //Регистрируем нового участника в событии
            var user = await RegisterUser();
            SetToken(user.Token);
            await EventsApi.Join(createEventResponse.Id);

            //Начинаем событие
            SetToken(TestUser.Token);
            
            await EventsApi.SetStatus(new SetStatusRequest<EventStatus>
            {
                Id = createEventResponse.Id,
                Status = EventStatus.Started
            });
        }
    }
}