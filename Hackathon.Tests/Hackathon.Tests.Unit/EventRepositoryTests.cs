using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Hackathon.Common.Abstraction;
using Hackathon.Common.Entities;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Event;
using Hackathon.DAL.Repositories;
using Hackathon.Tests.Common;
using Xunit;

namespace Hackathon.Tests.Unit
{
    public class EventRepositoryTests: BaseUnitTests
    {
        private readonly IEventRepository _eventRepository;

        public EventRepositoryTests()
        {
            _eventRepository = new EventRepository(Mapper, DbContext);
        }

        [Fact]
        public async Task ExistAsync_ShouldReturn_Success()
        {
            var newEventEntity = TestFaker.GetEventEntities(1).First();

            await DbContext.Events.AddAsync(newEventEntity);
            await DbContext.SaveChangesAsync();

            var isExist = await _eventRepository.ExistAsync(newEventEntity.Id);
            isExist.Should().BeTrue();
        }

        [Fact]
        public async Task CreateAsync_ShouldReturn_Id()
        {
            var createEventModel = TestFaker.GetCreateEventModels(1).FirstOrDefault();
            var createdEventId = await _eventRepository.CreateAsync(createEventModel);
            createdEventId.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task GetAsync_ShouldReturn_NotNull()
        {
            var createEventModel = TestFaker.GetCreateEventModels(1).FirstOrDefault();
            var eventId = await _eventRepository.CreateAsync(createEventModel);
            var eventModel = await _eventRepository.GetAsync(eventId);
            eventModel.Should().NotBeNull();
        }

        [Fact]
        public async Task GetAsync_WithGetFilterModel_ShouldReturn_Success()
        {
            var list = new List<EventEntity>
            {
                new() { Name = Guid.NewGuid().ToString(), Start = DateTime.UtcNow, Status = EventStatus.Draft },
                new() { Name = Guid.NewGuid().ToString(), Start = DateTime.UtcNow, Status = EventStatus.Draft },
                new() { Name = Guid.NewGuid().ToString(), Start = DateTime.UtcNow, Status = EventStatus.Draft },
                new() { Name = Guid.NewGuid().ToString(), Start = DateTime.UtcNow, Status = EventStatus.Draft }
            };

            await DbContext.AddRangeAsync(list);
            await DbContext.SaveChangesAsync();

            var response = await _eventRepository.GetAsync(new GetFilterModel<EventFilterModel>
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
                        .Excluding(x=>x.Teams));
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturn_Success()
        {
            var createEventModel = TestFaker.GetCreateEventModels(1).FirstOrDefault();
            var eventId = await _eventRepository.CreateAsync(createEventModel);

            var eventModel = await _eventRepository.GetAsync(eventId);
            var newName = Guid.NewGuid().ToString();
            eventModel.Name = newName;

            DbContext.ChangeTracker.Clear();
            await _eventRepository.UpdateAsync(eventModel);

            eventModel = await _eventRepository.GetAsync(eventId);
            eventModel.Name.Should().Be(newName);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDelete()
        {
            var createEventModel = TestFaker.GetCreateEventModels(1).FirstOrDefault();
            var eventId = await _eventRepository.CreateAsync(createEventModel);
            DbContext.ChangeTracker.Clear();
            await _eventRepository.DeleteAsync(eventId);
            var eventModel = await _eventRepository.GetAsync(eventId);
            eventModel.Should().BeNull();
        }


    }
}