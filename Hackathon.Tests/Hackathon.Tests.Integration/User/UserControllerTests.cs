using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Hackathon.Contracts.Requests.User;
using Hackathon.Tests.Common;
using Hackathon.Tests.Integration.Base;
using Mapster;
using Xunit;

namespace Hackathon.Tests.Integration.User
{
    public class UserControllerTests: BaseIntegrationTest, IClassFixture<TestWebApplicationFactory>
    {
        public UserControllerTests(TestWebApplicationFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task SignUp_Should_Success()
        {
            var fakeRequest = Mapper.Map<SignUpRequest>(TestFaker.GetSignUpModels(1).First());

            var response = await ApiService.Users.SignUpAsync(fakeRequest);

            Assert.NotNull(response);

            var userModel = await UserRepository.GetAsync(response.Id);

            Assert.NotNull(userModel);

            userModel.Should().BeEquivalentTo(fakeRequest, options=>
                options.Excluding(x=>x.Password));
        }

        [Fact]
        public async Task SignIn_ShouldReturn_AuthToken()
        {
            var fakeSignUpRequest = Mapper.Map<SignUpRequest>(TestFaker.GetSignUpModels(1).First());

            var signUpResponse = await ApiService.Users.SignUpAsync(fakeSignUpRequest);

            Assert.NotNull(signUpResponse);

            var signInRequest = Mapper.Map<SignInRequest>(fakeSignUpRequest);

            var signInResponse = await ApiService.Users.SignInAsync(signInRequest);

            Assert.NotNull(signInResponse);

            signInResponse.UserId.Should().Be(signUpResponse.Id);
            signInResponse.Expires.Should().BeAfter(DateTime.UtcNow);
            signInResponse.Token.Should().NotBeNullOrWhiteSpace();
        }
    }
}