using System;
using System.Linq;
using System.Threading.Tasks;
using Hackathon.Chats.Abstractions.Models.Events;
using Hackathon.Common.Models.Event;
using Hackathon.Common.Models.User;
using Hackathon.Contracts.Requests.Event;
using Xunit;

namespace Hackathon.Tests.Integration.Chat;

public class EventChatApiTests: BaseIntegrationTest
{
    public EventChatApiTests(TestWebApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task SendAsync_Should_Success()
    {
        //arrange
        var eventModel = TestFaker.GetEventModels(1, TestUser.Id).First();
        var createEventRequest = Mapper.Map<CreateEventRequest>(eventModel);

        var (eventOwnerId, eventOwnerToken) = await RegisterUser();
        SetToken(eventOwnerToken);
        var createEventResponse = await EventsApi.Create(createEventRequest);

        // Публикуем событие, чтобы можно было регистрироваться участникам
        await EventsApi.SetStatus(new SetStatusRequest<EventStatus>
        {
            Id = createEventResponse.Id,
            Status = EventStatus.OnModeration
        });

        var administrator = await RegisterUser(UserRole.Administrator);
        SetToken(administrator.Token);

        var getEventApiResponse = await EventsApi.Get(createEventResponse.Id);
        await ApprovalApplicationApiClient.Approve(getEventApiResponse.Content!.ApprovalApplicationId.GetValueOrDefault());

        SetToken(eventOwnerToken);
        await EventsApi.SetStatus(new SetStatusRequest<EventStatus>
        {
            Id = createEventResponse.Id,
            Status = EventStatus.Published
        });

        SetToken(TestUser.Token);

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

        var newMessage = new NewEventChatMessage
        {
            Message = Guid.NewGuid().ToString(),
            EventId = createEventResponse.Id,
            UserId = eventOwnerId
        };

        //act
        await EventChatApiClient.SendAsync(newMessage);

        //assert
        var messages = await EventChatRepository.GetMessagesAsync(createEventResponse.Id);

        var messageFromRepository = messages?.Items?.FirstOrDefault();

        Assert.NotNull(messageFromRepository);
        Assert.Equal(newMessage.Message, messageFromRepository.Message);
        Assert.Equal(TestUser.Id, messageFromRepository.OwnerId);
        Assert.Equal(eventOwnerId, messageFromRepository.UserId);
    }
}
