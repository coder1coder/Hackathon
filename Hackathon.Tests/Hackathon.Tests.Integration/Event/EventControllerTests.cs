using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
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
            var createEventModel = TestFaker.GetEventModels(1).First();

            var eventId = await EventRepository.CreateAsync(createEventModel);
            var eventModel = await ApiService.Events.Get(eventId);

            Assert.NotNull(eventModel);

            eventModel.Id.Should().Be(eventId);

            eventModel.Should().BeEquivalentTo(createEventModel, options =>
                options.Excluding(x => x.Id)
                    .Using<DateTime>(x=>
                        x.Subject.Should().BeCloseTo(x.Expectation, TimeSpan.FromMilliseconds(1)
                        )).WhenTypeIs<DateTime>());
        }
    }
}