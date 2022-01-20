using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Hackathon.Contracts.Requests.User;
using Hackathon.Tests.Integration.Base;
using Xunit;

namespace Hackathon.Tests.Integration.Auth
{
    public class AuthControllerTests: BaseIntegrationTest, IClassFixture<TestWebApplicationFactory>
    {
        public AuthControllerTests(TestWebApplicationFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task SignIn_ShouldReturn_AuthToken()
        {
            var fakeSignUpRequest = Mapper.Map<SignUpRequest>(TestFaker.GetSignUpModels(1).First());

            var signUpResponse = await UsersApi.SignUp(fakeSignUpRequest);

            Assert.NotNull(signUpResponse);

            var signInRequest = Mapper.Map<SignInRequest>(fakeSignUpRequest);

            var signInResponse =  await AuthApi.SignIn(signInRequest);

            Assert.NotNull(signInResponse);

            signInResponse.UserId.Should().Be(signUpResponse.Id);
            signInResponse.Expires.Should().BeGreaterThan(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
            signInResponse.Token.Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public async Task Should_Create_DefaultAdministratorUser()
        {
            var adminSignInResponse =  await AuthApi.SignIn(new SignInRequest()
            {
                UserName = AdministratorDefaultsConfig.Login,
                Password = AdministratorDefaultsConfig.Password,
            });

            Assert.NotNull(adminSignInResponse);
        }
    }
}