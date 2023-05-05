using Hackathon.Common.Models.Chat.Team;
using Hackathon.Common.Models.Team;
using Hackathon.Contracts.Requests.Team;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Hackathon.Tests.Integration.Chat;

public class TeamChatApiTests: BaseIntegrationTest
{
    public TeamChatApiTests(TestWebApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task SendAsync_Should_Success()
    {
        //arrange
        var newUser = await RegisterUser();
        SetToken(newUser.Token);

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
            UserId = newUser.Id
        };

        //act
        await TeamChatApiClient.SendAsync(newMessage);

        //assert
        var messages = await TeamChatRepository.GetMessagesAsync(teamId);

        var messageFromRepository = messages?.Items?.FirstOrDefault();

        Assert.NotNull(messageFromRepository);
        Assert.Equal(newMessage.Message, messageFromRepository.Message);
        Assert.Equal(newUser.Id, messageFromRepository.OwnerId);
        Assert.Equal(newUser.Id, messageFromRepository.UserId);
    }
}
