using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Event;
using Hackathon.DAL.Entities;
using Hackathon.Tests.Integration.Base;
using Xunit;

namespace Hackathon.Tests.Integration.Event
{
    public class EventRepositoryTests: BaseIntegrationTest, IClassFixture<TestWebApplicationFactory>
    {
        public EventRepositoryTests(TestWebApplicationFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task CreateAsync_ShouldReturn_Id()
        {
            var createEventModel = TestFaker.GetCreateEventModels(1, UserId).FirstOrDefault();
            Assert.NotNull(createEventModel);
            var createdEventId = await EventRepository.CreateAsync(createEventModel);
            createdEventId.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task GetAsync_ShouldReturn_NotNull()
        {
            var createEventModel = TestFaker.GetCreateEventModels(1, UserId).FirstOrDefault();
            Assert.NotNull(createEventModel);
            var eventId = await EventRepository.CreateAsync(createEventModel);
            var eventModel = await EventRepository.GetAsync(eventId);
            eventModel.Should().NotBeNull();
        }

        [Fact]
        public async Task GetAsync_WithGetFilterModel_ShouldReturn_Success()
        {
            var list = new List<EventEntity>
            {
                new() { Name = Guid.NewGuid().ToString(), Start = DateTime.UtcNow, Status = EventStatus.Draft, UserId = UserId},
                new() { Name = Guid.NewGuid().ToString(), Start = DateTime.UtcNow, Status = EventStatus.Draft, UserId = UserId },
                new() { Name = Guid.NewGuid().ToString(), Start = DateTime.UtcNow, Status = EventStatus.Draft, UserId = UserId },
                new() { Name = Guid.NewGuid().ToString(), Start = DateTime.UtcNow, Status = EventStatus.Draft, UserId = UserId }
            };

            await DbContext.AddRangeAsync(list);
            await DbContext.SaveChangesAsync();

            var response = await EventRepository.GetAsync(new GetListModel<EventFilterModel>
            {
                Filter = new EventFilterModel
                {
                    Name = list.First().Name
                }
            });

            response.TotalCount.Should().Be(1);
            response.Items
                .First()
                .Should()
                .BeEquivalentTo(list.First(), options =>
                    options.Using<DateTime>(x=>
                        x.Subject.Should().BeCloseTo(x.Expectation, TimeSpan.FromMilliseconds(10)))
                        .WhenTypeIs<DateTime>()
                        .Excluding(x=>x.TeamEvents)
                        .Excluding(x=>x.User)
                    );
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturn_Success()
        {
            var createEventModel = TestFaker.GetCreateEventModels(1, UserId).FirstOrDefault();
            Assert.NotNull(createEventModel);
            var eventId = await EventRepository.CreateAsync(createEventModel);

            var eventModel = await EventRepository.GetAsync(eventId);
            var newName = Guid.NewGuid().ToString();
            eventModel.Name = newName;

            DbContext.ChangeTracker.Clear();
            await EventRepository.UpdateAsync(new []{ eventModel });

            eventModel = await EventRepository.GetAsync(eventId);
            eventModel.Name.Should().Be(newName);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDelete()
        {
            var createEventModel = TestFaker.GetCreateEventModels(1, UserId).FirstOrDefault();
            Assert.NotNull(createEventModel);
            var eventId = await EventRepository.CreateAsync(createEventModel);
            DbContext.ChangeTracker.Clear();
            await EventRepository.DeleteAsync(eventId);
            var eventModel = await EventRepository.GetAsync(eventId);
            eventModel.Should().BeNull();
        }

        [Fact]
        public async Task ExistAsync_ShouldReturn_Success()
        {
            var signUpModel = TestFaker.GetSignUpModels(1).First();
            var createdUserId = await UserRepository.CreateAsync(signUpModel);

            var exist = await UserRepository.ExistAsync(createdUserId);
            exist.Should().BeTrue();
        }
    }
}