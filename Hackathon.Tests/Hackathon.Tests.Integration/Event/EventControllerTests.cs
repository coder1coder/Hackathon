using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Hackathon.Common.Models.Event;
using Hackathon.Contracts.Requests.Event;
using Hackathon.Tests.Common;
using Hackathon.Tests.Integration.Base;
using Xunit;

namespace Hackathon.Tests.Integration.Event
{
    public class EventControllerTests: BaseIntegrationTest, IClassFixture<TestWebApplicationFactory>
    {
        public EventControllerTests(TestWebApplicationFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task Get_ShouldReturn_Success()
        {
            var eventModel = TestFaker.GetEventModels(1).First();
            var createEventModel = Mapper.Map<CreateEventModel>(eventModel);

            var eventId = await EventRepository.CreateAsync(createEventModel);
            eventModel = await ApiService.Events.Get(eventId);

            Assert.NotNull(eventModel);

            eventModel.Id.Should().Be(eventId);

            eventModel.Should().BeEquivalentTo(createEventModel, options =>
                options
                    .Using<DateTime>(x=>
                        x.Subject.Should().BeCloseTo(x.Expectation, TimeSpan.FromMilliseconds(1)
                        )).WhenTypeIs<DateTime>());
        }

        [Fact]
        public async Task SetStatus_ShouldReturn_Success()
        {
            var eventModel = TestFaker.GetEventModels(1).First();
            var createEventModel = Mapper.Map<CreateEventModel>(eventModel);

            var eventId = await EventRepository.CreateAsync(createEventModel);

            await FluentActions
                .Invoking(async () => await ApiService.Events.SetStatus(new SetStatusRequest<EventStatus>
                {
                    Id = eventId,
                    Status = EventStatus.Published
                }))
                .Should()
                .NotThrowAsync();

            eventModel = await ApiService.Events.Get(eventId);
            eventModel.Status.Should().Be(EventStatus.Published);

            await FluentActions
                .Invoking(async () => await ApiService.Events.SetStatus(new SetStatusRequest<EventStatus>
                {
                    Id = eventId,
                    Status = EventStatus.Archived
                }))
                .Should()
                .NotThrowAsync();

            eventModel = await ApiService.Events.Get(eventId);
            eventModel.Status.Should().Be(EventStatus.Archived);
        }
    }
}