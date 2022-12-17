using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Hackathon.Contracts.Requests.User;
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

    }
}
