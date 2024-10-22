using System;
using System.Linq;
using System.Threading.Tasks;
using Hackathon.API.Contracts.Teams;
using Hackathon.Chats.Abstractions.Models.Teams;
using Hackathon.Common.Models.Teams;
using Xunit;

namespace Hackathon.Tests.Integration.Chats;

public class TeamChatApiTests: BaseIntegrationTest
{
    public TeamChatApiTests(TestWebApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task SendAsync_Should_Success()
    {
        //arrange
        var (userId, authToken) = await RegisterUser();
        SetToken(authToken);

        var createTeamResponse = await TeamApiClient.CreateAsync(new CreateTeamRequest
        {
            Name = Guid.NewGuid().ToString()[..30],
            Type = TeamType.Public
        });

        var teamId = createTeamResponse.Content?.Id ?? default;

        var newMessage = new NewTeamChatMessage
        {
            Message = Guid.NewGuid().ToString(),
            TeamId = teamId,
            UserId = userId
        };

        //act
        await TeamChatApiClient.SendAsync(newMessage);

        //assert
        var messages = await TeamChatRepository.GetMessagesAsync(teamId);

        var messageFromRepository = messages?.Items?.FirstOrDefault();

        Assert.NotNull(messageFromRepository);
        Assert.Equal(newMessage.Message, messageFromRepository.Message);
        Assert.Equal(userId, messageFromRepository.OwnerId);
        Assert.Equal(userId, messageFromRepository.UserId);
    }
}
