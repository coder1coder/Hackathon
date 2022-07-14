using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Hackathon.BL.User;
using Hackathon.Common.Models.User;
using Hackathon.Contracts.Requests.User;
using Hackathon.Tests.Integration.Base;
using Refit;
using Xunit;

namespace Hackathon.Tests.Integration.User
{
    public class UserControllerTests: BaseIntegrationTest
    {
        public UserControllerTests(TestWebApplicationFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task SignUp_Should_Success()
        {
            var fakeRequest = Mapper.Map<SignUpRequest>(TestFaker.GetSignUpModels(1).First());

            var response = await UsersApi.SignUp(fakeRequest);

            Assert.NotNull(response);

            var userModel = await UserRepository.GetAsync(response.Id);

            Assert.NotNull(userModel);

            userModel.Should().BeEquivalentTo(fakeRequest, options=>
                options.Excluding(x=>x.Password));
        }

        [Fact]
        public async Task GetReactions_Should_ReturnArray()
        {
            //arrange
            var (targetUserId, _) = await RegisterUser();

            await UsersApi.AddReaction(targetUserId, UserProfileReaction.Like);

            //act
            var response = await UsersApi.GetReactions(targetUserId);

            //assert
            Assert.NotNull(response);
            response.Should().ContainSingle(x => x == UserProfileReaction.Like);
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
        public async Task AddReaction_WhenReactionAlreadyExist_Should_Throw_ValidationException()
        {
            //arrange
            const UserProfileReaction reaction = UserProfileReaction.Like;

            var (targetUserId, _) = await RegisterUser();

            await UsersApi.AddReaction(targetUserId, reaction);

            //act
            //assert
            await FluentActions.Invoking(async () => await UsersApi.AddReaction(targetUserId, reaction))
                .Should()
                .ThrowAsync<ValidationApiException>()
                .Where(x =>
                    x.Content != null
                    && x.Content.Status == 400
                    && x.Content.Detail == UserService.ReactionAlreadyExistMessage);
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
                    && x.Content.Status == 400
                    && x.Content.Detail == UserService.ReactionNotExistMessage);
        }
    }
}
