using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Hackathon.Contracts.Requests.User;
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
            var fakeRequest = TestFaker.GetSignUpModels(1).First().Adapt<SignUpRequest>();

            var response = await ApiService.Users.CreateAsync(fakeRequest);

            Assert.NotNull(response);

            var userModel = await UserRepository.GetAsync(response.Id);

            Assert.NotNull(userModel);

            userModel.Should().BeEquivalentTo(fakeRequest, options=>
                options.Excluding(x=>x.Password));
        }
    }
}