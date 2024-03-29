using System.Threading.Tasks;
using FluentAssertions;
using Hackathon.Common.Models.Friend;
using Xunit;

namespace Hackathon.Tests.Integration.Friendship;

public class FriendshipControllerTests : BaseIntegrationTest
{
    public FriendshipControllerTests(TestWebApplicationFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task CreateOrAcceptOffer_Create_Should_Success()
    {
        var (secondUserId, _) = await RegisterUser();

        await FluentActions.Invoking(async () => await FriendshipApi.CreateOrAcceptOffer(secondUserId))
            .Should()
            .NotThrowAsync();

        var offer = await FriendshipRepository.GetOfferAsync(TestUser.Id, secondUserId, GetOfferOption.Outgoing);

        Assert.NotNull(offer);
        offer.Status.Should().Be(FriendshipStatus.Pending);
    }

    [Fact]
    public async Task CreateOrAcceptOffer_Accept_Should_Success()
    {
        var (secondUserId, secondUserToken) = await RegisterUser();

        await FriendshipApi.CreateOrAcceptOffer(secondUserId);

        SetToken(secondUserToken);

        await FluentActions.Invoking(async () => await FriendshipApi.CreateOrAcceptOffer(TestUser.Id))
            .Should()
            .NotThrowAsync();

        var offer = await FriendshipRepository.GetOfferAsync(TestUser.Id, secondUserId, GetOfferOption.Outgoing);

        Assert.NotNull(offer);
        offer.Status.Should().Be(FriendshipStatus.Confirmed);
    }

    [Fact]
    public async Task RejectOffer_Should_Success()
    {
        var (secondUserId, secondUserToken) = await RegisterUser();

        await FriendshipApi.CreateOrAcceptOffer(secondUserId);

        SetToken(secondUserToken);

        await FluentActions.Invoking(async () => await FriendshipApi.RejectOffer(TestUser.Id))
            .Should()
            .NotThrowAsync();

        var offer = await FriendshipRepository.GetOfferAsync(TestUser.Id, secondUserId, GetOfferOption.Outgoing);

        Assert.NotNull(offer);
        offer.Status.Should().Be(FriendshipStatus.Rejected);
    }

    /// <summary>
    /// Если пользователь прекращает дружбу, то статус дружбы должен быть установлен в <see cref="FriendshipStatus.Pending"/>
    /// Один пользователь остается в подписчиках у другого
    /// </summary>
    [Fact]
    public async Task EndFriendship_Should_SetFriendshipStatus_Pending()
    {
        //arrange
        var (secondUserId, secondUserToken) = await RegisterUser();

        await FriendshipApi.CreateOrAcceptOffer(secondUserId);

        SetToken(secondUserToken);

        await FriendshipApi.CreateOrAcceptOffer(TestUser.Id);

        //act
        await FriendshipApi.EndFriendship(TestUser.Id);

        //assert
        var offer = await FriendshipRepository.GetOfferAsync(TestUser.Id, secondUserId);

        Assert.NotNull(offer);
        offer.Status.Should().Be(FriendshipStatus.Pending);
    }
}
