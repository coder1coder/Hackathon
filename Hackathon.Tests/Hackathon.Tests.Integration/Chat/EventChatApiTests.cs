using Hackathon.Common.Models.Chat.Event;
using Hackathon.Contracts.Requests.Event;
using System;
using System.Linq;
using System.Threading.Tasks;
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
        var newUser = await RegisterUser();
        SetToken(newUser.Token);

        var eventModel = TestFaker.GetEventModels(1, TestUser.Id).First();
        var createEventRequest = Mapper.Map<CreateEventRequest>(eventModel);

        var eventCreateResponse = await EventsApi.Create(createEventRequest);

        var newMessage = new NewEventChatMessage
        {
            Message = Guid.NewGuid().ToString(),
            EventId = eventCreateResponse.Id,
            UserId = newUser.Id
        };

        //act
        await EventChatApiClient.SendAsync(newMessage);

        //assert
        var messages = await EventChatRepository.GetMessagesAsync(eventCreateResponse.Id);

        var messageFromRepository = messages?.Items?.FirstOrDefault();

        Assert.NotNull(messageFromRepository);
        Assert.Equal(newMessage.Message, messageFromRepository.Message);
        Assert.Equal(newUser.Id, messageFromRepository.OwnerId);
        Assert.Equal(newUser.Id, messageFromRepository.UserId);
    }
}
