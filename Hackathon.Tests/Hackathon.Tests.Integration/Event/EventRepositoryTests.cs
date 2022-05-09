using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Event;
using Hackathon.Entities;
using Hackathon.Tests.Integration.Base;
using Xunit;

namespace Hackathon.Tests.Integration.Event
{
    public class EventRepositoryTests: BaseIntegrationTest
    {
        public EventRepositoryTests(TestWebApplicationFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task CreateAsync_ShouldReturn_Id()
        {
            var createEventModel = TestFaker.GetCreateEventModels(1, TestUser.Id).FirstOrDefault();
            Assert.NotNull(createEventModel);
            var createdEventId = await EventRepository.CreateAsync(createEventModel);
            createdEventId.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task GetAsync_ShouldReturn_NotNull()
        {
            var createEventModel = TestFaker.GetCreateEventModels(1, TestUser.Id).FirstOrDefault();
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
                new() { Name = Guid.NewGuid().ToString(), Start = DateTime.UtcNow, Status = EventStatus.Draft, OwnerId = TestUser.Id},
                new() { Name = Guid.NewGuid().ToString(), Start = DateTime.UtcNow, Status = EventStatus.Draft, OwnerId = TestUser.Id },
                new() { Name = Guid.NewGuid().ToString(), Start = DateTime.UtcNow, Status = EventStatus.Draft, OwnerId = TestUser.Id },
                new() { Name = Guid.NewGuid().ToString(), Start = DateTime.UtcNow, Status = EventStatus.Draft, OwnerId = TestUser.Id }
            };

            await DbContext.AddRangeAsync(list);
            await DbContext.SaveChangesAsync();

            var response = await EventRepository.GetListAsync(TestUser.Id, new GetListParameters<EventFilter>
            {
                Filter = new EventFilter
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
                        .Excluding(x=>x.Teams)
                        .Excluding(x=>x.Owner)
                    );
        }

        [Fact(Skip = "TODO: fix")]
        public async Task UpdateAsync_ShouldReturn_Success()
        {
            var createEventModel = TestFaker.GetCreateEventModels(1, TestUser.Id).FirstOrDefault();
            Assert.NotNull(createEventModel);
            var eventId = await EventRepository.CreateAsync(createEventModel);

            var eventModel = await EventRepository.GetAsync(eventId);
            var newName = Guid.NewGuid().ToString();
            eventModel.Name = newName;

            DbContext.ChangeTracker.Clear();
            // await EventRepository.UpdateAsync(eventModel);

            eventModel = await EventRepository.GetAsync(eventId);
            eventModel.Name.Should().Be(newName);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDelete()
        {
            var createEventModel = TestFaker.GetCreateEventModels(1, TestUser.Id).FirstOrDefault();
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
            var createdUser = await UserRepository.CreateAsync(signUpModel);

            var exist = await UserRepository.ExistAsync(createdUser.Id);
            exist.Should().BeTrue();
        }
    }
}