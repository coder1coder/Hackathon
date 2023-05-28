using Bogus;
using FluentAssertions;
using Hackathon.Common.Models.Event;
using Hackathon.Common.Models.EventStage;
using Hackathon.Contracts.Requests.Event;
using System;
using System.Linq;
using System.Threading.Tasks;
using Refit;
using Xunit;

namespace Hackathon.Tests.Integration.Event;

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
        var createEventRequest = Mapper.Map<CreateEventRequest>(eventModel);

        var baseCreateResponse = await EventsApi.Create(createEventRequest);
        var getEventResponse = await EventsApi.Get(baseCreateResponse.Id);
        eventModel = getEventResponse.Content;

        Assert.NotNull(eventModel);

        eventModel.Id.Should().Be(baseCreateResponse.Id);

        eventModel.Should().BeEquivalentTo(createEventRequest, options =>
            options
                .Excluding(x=>x.Stages)
                .Using<DateTime>(x =>
                    x.Subject.Should().BeCloseTo(x.Expectation, TimeSpan.FromMilliseconds(1)))
                .WhenTypeIs<DateTime>());

        eventModel.Stages.Should().BeEquivalentTo(createEventRequest.Stages, options =>
            options.Excluding(x=>x.EventId));
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
        var @event = TestFaker.GetEventModels(1, TestUser.Id, EventStatus.Draft).First();
        var request = Mapper.Map<CreateEventRequest>(@event);
        var createEventResponse = await EventsApi.Create(request);

        // Публикуем событие, чтобы можно было регистрироваться участникам
        await EventsApi.SetStatus(new SetStatusRequest<EventStatus>
        {
            Id = createEventResponse.Id,
            Status = EventStatus.Published
        });

        // Присоединяемся к событию в качестве участника
        await EventsApi.JoinAsync(createEventResponse.Id);

        // Регистрируем нового участника в событии
        var user = await RegisterUser();
        SetToken(user.Token);
        await EventsApi.JoinAsync(createEventResponse.Id);

        // Начинаем событие
        SetToken(TestUser.Token);

        await EventsApi.SetStatus(new SetStatusRequest<EventStatus>
        {
            Id = createEventResponse.Id,
            Status = EventStatus.Started
        });
    }

    [Fact]
    public async Task GoNextStage_ShouldSuccess()
    {
        //arrange
        var @event = TestFaker.GetEventModels(1, TestUser.Id).First();

        @event.Stages = new Faker<EventStageModel>()
            .RuleFor(x => x.Name, f => f.Random.String2(10))
            .RuleFor(x => x.Duration, f => f.Random.Int(1, 10))
            .Generate(3)
            .ToList();

        var request = Mapper.Map<CreateEventRequest>(@event);
        var createEventResponse = await EventsApi.Create(request);

        await EventsApi.SetStatus(new SetStatusRequest<EventStatus>
        {
            Id = createEventResponse.Id,
            Status = EventStatus.Published
        });

        await EventsApi.JoinAsync(createEventResponse.Id);

        var user = await RegisterUser();
        SetToken(user.Token);
        await EventsApi.JoinAsync(createEventResponse.Id);

        SetToken(TestUser.Token);

        await EventsApi.SetStatus(new SetStatusRequest<EventStatus>
        {
            Id = createEventResponse.Id,
            Status = EventStatus.Started
        });

        //act
        await EventsApi.GoNextStage(createEventResponse.Id);

        //assert
        var getEventResponse = await EventsApi.Get(createEventResponse.Id);

        Assert.NotNull(getEventResponse.Content);
        getEventResponse.Content.CurrentStageId.Should()
            .Be(getEventResponse.Content.Stages.OrderBy(x => x.Order).ElementAt(1).Id);

    }

    [Fact]
    public async Task Create_Should_Store_Tasks()
    {
        //arrange
        var eventModel = TestFaker.GetEventModels(1, TestUser.Id).First();
        var createEventRequest = Mapper.Map<CreateEventRequest>(eventModel);
        var createEventResponse = await EventsApi.Create(createEventRequest);

        //act
        var eventCreationResponse = await EventsApi.Get(createEventResponse.Id);

        //assert
        Assert.NotNull(eventCreationResponse.Content?.Tasks);
        eventCreationResponse.Content.Tasks.Should().HaveCount(TestFaker.EventTasksAmount);
        eventCreationResponse.Content.Tasks.Should().NotContainNulls();
    }

    [Fact]
    public async Task UploadEventImage_Should_Return_Valid_Guid()
    {
        //arrange
        var file = TestFaker.GetFormFile();
        var streamPath = new StreamPart(file.OpenReadStream(), file.FileName, file.ContentType, file.Name);
        
        //act
        var uploadFileId = await EventsApi.UploadEventImage(streamPath);
        
        //assert
        Assert.NotEqual(uploadFileId, Guid.Empty);
    }
}
