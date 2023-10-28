using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using Hackathon.BL.Validation.Event;
using Hackathon.BL.Validation.ImageFile;
using Hackathon.Common.Models.Event;
using Hackathon.Common.Models.EventStage;
using Hackathon.Common.Models.User;
using Hackathon.Contracts.Requests.Event;
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
        //arrange
        var eventModel = TestFaker.GetEventModels(1, TestUser.Id).First();
        var createEventRequest = Mapper.Map<CreateEventRequest>(eventModel);
        createEventRequest.ImageId = await GetEventImageId();
        var baseCreateResponse = await EventsApi.Create(createEventRequest);

        //act
        var getEventResponse = await EventsApi.Get(baseCreateResponse.Id);

        //assert
        Assert.NotNull(getEventResponse.Content);

        getEventResponse.Content.Id.Should().Be(baseCreateResponse.Id);
        getEventResponse.Content.Should().BeEquivalentTo(createEventRequest, options =>
            options
                .Excluding(x=>x.Stages)
                .Excluding(x=>x.Agreement)
                .Using<DateTime>(x =>
                    x.Subject.Should().BeCloseTo(x.Expectation, TimeSpan.FromMilliseconds(1)))
                .WhenTypeIs<DateTime>());

        getEventResponse.Content.Stages.Should().BeEquivalentTo(createEventRequest.Stages, options =>
            options.Excluding(x=>x.EventId));
        getEventResponse.Content.Agreement.Should().BeEquivalentTo(createEventRequest.Agreement);
    }

    [Fact]
    public async Task SetStatus_FromDraft_ToOnModeration_ShouldReturn_Success()
    {
        var eventModel = TestFaker.GetEventModels(1, TestUser.Id).First();
        var createEventRequest = Mapper.Map<CreateEventRequest>(eventModel);
        createEventRequest.ImageId = await GetEventImageId();
        var createEventResponse = await EventsApi.Create(createEventRequest);

        await FluentActions
            .Invoking(async () => await EventsApi.SetStatus(new SetStatusRequest<EventStatus>
            {
                Id = createEventResponse.Id,
                Status = EventStatus.OnModeration
            }))
            .Should()
            .NotThrowAsync();

        var getEventResponse = await EventsApi.Get(createEventResponse.Id);
        eventModel = getEventResponse.Content;

        Assert.NotNull(eventModel);
        Assert.Equal(EventStatus.OnModeration, eventModel.Status);
    }

    [Fact]
    public async Task SetStatus_FromDraft_ToOnModeration_ShouldCreate_Application()
    {
        //arrange
        var eventModel = TestFaker.GetEventModels(1, TestUser.Id).First();
        var createEventRequest = Mapper.Map<CreateEventRequest>(eventModel);
        createEventRequest.ImageId = await GetEventImageId();
        var createEventResponse = await EventsApi.Create(createEventRequest);

        //act
        await EventsApi.SetStatus(new SetStatusRequest<EventStatus>
        {
            Id = createEventResponse.Id,
            Status = EventStatus.OnModeration
        });

        var getEventResponse = await EventsApi.Get(createEventResponse.Id);
        eventModel = getEventResponse.Content;

        //assert
        Assert.NotNull(eventModel);
        Assert.Equal(EventStatus.OnModeration, eventModel.Status);
        Assert.True(eventModel.ApprovalApplicationId.HasValue);
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
        request.ImageId = await GetEventImageId();
        var createEventResponse = await EventsApi.Create(request);

        await EventsApi.SetStatus(new SetStatusRequest<EventStatus>
        {
            Id = createEventResponse.Id,
            Status = EventStatus.OnModeration
        });

        var administrator = await RegisterUser(UserRole.Administrator);
        SetToken(administrator.Token);

        var getEventApiResponse = await EventsApi.Get(createEventResponse.Id);
        await ApprovalApplicationApiClient.Approve(getEventApiResponse.Content!.ApprovalApplicationId.GetValueOrDefault());

        SetToken(TestUser.Token);

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
        createEventRequest.ImageId = await GetEventImageId();
        var createEventResponse = await EventsApi.Create(createEventRequest);

        //act
        var eventCreationResponse = await EventsApi.Get(createEventResponse.Id);

        //assert
        Assert.NotNull(eventCreationResponse);
        Assert.NotNull(eventCreationResponse.Content);
        Assert.NotNull(eventCreationResponse.Content?.Tasks);
        eventCreationResponse.Content.Tasks.Should().HaveCount(TestFaker.EventTasksAmount);
        eventCreationResponse.Content.Tasks.Should().NotContainNulls();
    }

    [Fact]
    public async Task UploadEventImage_Should_Return_Valid_Guid()
    {
        //arrange
        await using var memoryStream = new MemoryStream();
        var file = TestFaker.GetEmptyImage(memoryStream, FileImageValidator.MinWidthEventImage, FileImageValidator.MinHeightEventImage);
        var streamPath = new StreamPart(file.OpenReadStream(), file.FileName, file.ContentType, file.Name);

        //act
        var uploadFileId = await EventsApi.UploadEventImage(streamPath);

        //assert
        Assert.NotEqual(uploadFileId, Guid.Empty);
    }

    [Fact]
    public async Task Create_Should_Store_Agreement()
    {
        //arrange
        var eventModel = TestFaker.GetEventModels(1, TestUser.Id).First();
        var createEventRequest = Mapper.Map<CreateEventRequest>(eventModel);
        createEventRequest.ImageId = await GetEventImageId();
        var createEventResponse = await EventsApi.Create(createEventRequest);

        //act
        var eventCreationResponse = await EventsApi.Get(createEventResponse.Id);

        //assert
        Assert.NotNull(eventCreationResponse);
        Assert.NotNull(eventCreationResponse.Content);
        Assert.NotNull(eventCreationResponse.Content.Agreement);

        eventCreationResponse.Content.Agreement
            .Should()
            .BeEquivalentTo(eventModel.Agreement, options =>
                options.Excluding(x=>x.EventId)
                    .Excluding(x=>x.Event));

        Assert.True(eventCreationResponse.Content.Agreement.EventId > default(long));
    }

    [Fact(DisplayName = "Обновление карточки мероприятия не владельцем")]
    public async Task Update_When_UserIsNotOwner()
    {
        //arrange
        var eventModel = TestFaker.GetEventModels(1, TestUser.Id).First();
        var createEventRequest = Mapper.Map<CreateEventRequest>(eventModel);
        createEventRequest.ImageId = await GetEventImageId();
        var createEventResponse = await EventsApi.Create(createEventRequest);

        //act
        var anotherUser = await RegisterUser();
        SetToken(anotherUser.Token);
        var updateEventRequest = Mapper.Map<CreateEventRequest, UpdateEventRequest>(createEventRequest);
        updateEventRequest.Id = createEventResponse.Id;
        var response = await EventsApi.Update(updateEventRequest);

        //assert
        Assert.NotNull(response);
        Assert.False(response.IsSuccessStatusCode);
        Assert.NotNull(response.Error);
        Assert.NotNull(response.Error.Content);
        Assert.Contains(EventErrorMessages.NoRightsExecuteOperation, response.Error.Content);
    }

    [Fact(DisplayName = "Событие в статусе <Черновик> не доступно для другого пользователя")]
    public async Task Drafted_Events_NotAvailable_For_AnotherUser()
    {
        //arrange
        var eventModel = TestFaker.GetEventModels(1, TestUser.Id, EventStatus.Draft).First();
        var createEventRequest = Mapper.Map<CreateEventRequest>(eventModel);
        createEventRequest.ImageId = await GetEventImageId();
        var createEventResponse = await EventsApi.Create(createEventRequest);

        //act
        var anotherUser = await RegisterUser();
        SetToken(anotherUser.Token);
        var response = await EventsApi.Get(createEventResponse.Id);

        //assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact(DisplayName = "Событие в статусе <Черновик> доступно для владельца")]
    public async Task Drafted_Events_Available_For_Owner()
    {
        //arrange
        var eventModel = TestFaker.GetEventModels(1, TestUser.Id).First();
        var createEventRequest = Mapper.Map<CreateEventRequest>(eventModel);
        createEventRequest.ImageId = await GetEventImageId();
        var createEventResponse = await EventsApi.Create(createEventRequest);

        //act
        var response = await EventsApi.Get(createEventResponse.Id);

        //assert
        Assert.True(response.IsSuccessStatusCode);
        Assert.NotNull(response.Content);
    }

    [Fact(DisplayName = "Событие в статусе <На модерации> не доступно для другого пользователя")]
    public async Task OnModeration_Events_NotAvailable_For_AnotherUser()
    {
        //arrange
        var eventModel = TestFaker.GetEventModels(1, TestUser.Id).First();
        var createEventRequest = Mapper.Map<CreateEventRequest>(eventModel);
        createEventRequest.ImageId = await GetEventImageId();
        var createEventResponse = await EventsApi.Create(createEventRequest);
        await EventsApi.SetStatus(new SetStatusRequest<EventStatus>
        {
            Id = createEventResponse.Id,
            Status = EventStatus.OnModeration
        });

        //act
        var anotherUser = await RegisterUser();
        SetToken(anotherUser.Token);
        var response = await EventsApi.Get(createEventResponse.Id);

        //assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact(DisplayName = "Событие в статусе <На модерации> доступно для владельца")]
    public async Task OnModeration_Events_Available_For_Owner()
    {
        //arrange
        var eventModel = TestFaker.GetEventModels(1, TestUser.Id).First();
        var createEventRequest = Mapper.Map<CreateEventRequest>(eventModel);
        createEventRequest.ImageId = await GetEventImageId();
        var createEventResponse = await EventsApi.Create(createEventRequest);
        await EventsApi.SetStatus(new SetStatusRequest<EventStatus>
        {
            Id = createEventResponse.Id,
            Status = EventStatus.OnModeration
        });

        //act
        var response = await EventsApi.Get(createEventResponse.Id);

        //assert
        Assert.True(response.IsSuccessStatusCode);
        Assert.NotNull(response.Content);
    }

    [Fact(DisplayName = "Событие в статусе <На модерации> доступно для администратора")]
    public async Task OnModeration_Events_Available_For_Administrator()
    {
        //arrange
        var eventModel = TestFaker.GetEventModels(1, TestUser.Id).First();
        var createEventRequest = Mapper.Map<CreateEventRequest>(eventModel);
        createEventRequest.ImageId = await GetEventImageId();
        var createEventResponse = await EventsApi.Create(createEventRequest);
        await EventsApi.SetStatus(new SetStatusRequest<EventStatus>
        {
            Id = createEventResponse.Id,
            Status = EventStatus.OnModeration
        });

        //act
        var administrator = await RegisterUser(UserRole.Administrator);
        SetToken(administrator.Token);
        var response = await EventsApi.Get(createEventResponse.Id);

        //assert
        Assert.True(response.IsSuccessStatusCode);
        Assert.NotNull(response.Content);
    }

    [Fact]
    public async Task Update_ShouldNot_RemoveEvent()
    {
        //arrange
        var eventModel = TestFaker.GetEventModels(1, TestUser.Id).First();
        var createEventRequest = Mapper.Map<CreateEventRequest>(eventModel);
        createEventRequest.ImageId = await GetEventImageId();
        var createEventResponse = await EventsApi.Create(createEventRequest);
        await EventsApi.SetStatus(new SetStatusRequest<EventStatus>
        {
            Id = createEventResponse.Id,
            Status = EventStatus.OnModeration
        });

        var updateEventRequest = Mapper.Map<CreateEventRequest, UpdateEventRequest>(createEventRequest);
        updateEventRequest.Id = createEventResponse.Id;
        foreach (var stage in updateEventRequest.Stages)
            stage.EventId = createEventResponse.Id;

        //act
        await EventsApi.Update(updateEventRequest);
        var getEventResponse = await EventsApi.Get(createEventResponse.Id);

        //assert
        Assert.NotNull(getEventResponse);
        Assert.True(getEventResponse.IsSuccessStatusCode);
        Assert.NotNull(getEventResponse.Content);
    }
}
