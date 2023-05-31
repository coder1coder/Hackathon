using System;
using FluentAssertions;
using System.Linq;
using System.Threading.Tasks;
using Hackathon.Contracts.Requests.User;
using Refit;
using Xunit;

namespace Hackathon.Tests.Integration.User;

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
            options
                .Excluding(x=>x.Password)
                .Excluding(x=>x.Email)
        );
    }

    [Fact]
    public async Task UploadProfileImage_Should_Return_Valid_Guid()
    {
        //arrange
        var file = TestFaker.GetFormFile();
        var streamPath = new StreamPart(file.OpenReadStream(), file.FileName, file.ContentType, file.Name);
        
        //act
        var uploadFileId = await UsersApi.UploadProfileImage(streamPath);
        
        //assert
        Assert.NotEqual(uploadFileId, Guid.Empty);
    }
}
