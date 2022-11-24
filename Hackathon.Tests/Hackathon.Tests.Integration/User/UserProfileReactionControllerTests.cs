using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Hackathon.BL.User;
using Hackathon.Common.Models.User;
using Hackathon.Tests.Integration.Base;
using Refit;
using StackExchange.Redis;
using Xunit;

namespace Hackathon.Tests.Integration.User;

public sealed class UserProfileReactionControllerTests: BaseIntegrationTest
{
    public UserProfileReactionControllerTests(TestWebApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task RemoveReaction_Should_Success()
    {
        //arrange
        const UserProfileReaction reaction = UserProfileReaction.Like;

        var (targetUserId, _) = await RegisterUser();

        await UsersApi.AddReaction(targetUserId, reaction);

        //act
        //assert
        await FluentActions.Invoking(async () => await UsersApi.RemoveReaction(targetUserId, reaction))
            .Should()
            .NotThrowAsync();
    }

    [Fact]
    public async Task GetReactions_Should_ReturnArray()
    {
        //arrange
        const UserProfileReaction reaction = UserProfileReaction.Like;
        var (targetUserId, _) = await RegisterUser();

        await UsersApi.AddReaction(targetUserId, reaction);

        //act
        var response = await UsersApi.GetReactions(targetUserId);

        //assert
        Assert.NotNull(response);
        response.Should().Be(reaction);
    }

    [Fact]
    public async Task AddReactions_Should_Success()
    {
        //arrange
        var (targetUserId, _) = await RegisterUser();

        //act
        //assert
        await FluentActions.Invoking(async () => await UsersApi.AddReaction(targetUserId, UserProfileReaction.Like))
            .Should()
            .NotThrowAsync();
    }

    [Fact]
    public async Task RemoveReaction_WhenReactionNotExist_Should_Throw_ValidationException()
    {
        //arrange
        const UserProfileReaction reaction = UserProfileReaction.Like;

        var (targetUserId, _) = await RegisterUser();

        //act
        //assert
        await FluentActions.Invoking(async () => await UsersApi.RemoveReaction(targetUserId, reaction))
            .Should()
            .ThrowAsync<ValidationApiException>()
            .Where(x =>
                x.Content != null
                && x.Content.Status == (int)HttpStatusCode.BadRequest
                && x.Content.Detail == UserProfileReactionService.ReactionNotExistMessage
                );
    }

}
