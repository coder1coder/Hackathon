using System.Linq;
using System.Threading.Tasks;
using Bogus;
using FluentValidation;
using Hackathon.Abstraction;
using Hackathon.BL.User;
using Hackathon.Common.Configuration;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.User;
using MapsterMapper;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace Hackathon.BL.Tests;

public class UserServiceTests
{

    [Fact]
    public async Task GetAsync_ShouldReturn_UserModelCollection()
    {
        //arrange
        var appSettingsMock = new Mock<IOptions<AppSettings>>();
        var signUpValidatorMock = new Mock<IValidator<SignUpModel>>();
        var signInValidatorMock = new Mock<IValidator<SignInModel>>();
        var userRepositoryMock = new Mock<IUserRepository>();

        var fakeUser = new Faker<UserModel>()
            .RuleFor(x => x.UserName, f => f.Name.FirstName())
            .RuleFor(x => x.Email, f => f.Internet.Email())
            .Generate();

        userRepositoryMock.Setup(x => x.GetAsync(It.IsAny<GetListModel<UserFilterModel>>()))
            .ReturnsAsync(new BaseCollectionModel<UserModel>
            {
                Items = new[] {fakeUser},
                TotalCount = 1
            });

        var sut = new UserService(
            appSettingsMock.Object,
            signUpValidatorMock.Object,
            signInValidatorMock.Object,
            null,
            userRepositoryMock.Object,
            null
            );

        //act
        var result = await sut.GetAsync(new GetListModel<UserFilterModel>()
        {
            Filter = new UserFilterModel
            {
                Username = fakeUser.UserName,
                Email = fakeUser.Email
            }
        });

        //assert
        Assert.NotNull(result);
        Assert.Equal(1, result.TotalCount);
        Assert.Equal(result.TotalCount, result.Items.Count);

        var first = result.Items.First();
        Assert.Equal(fakeUser.UserName, first.UserName);
        Assert.Equal(fakeUser.Email, first.Email);
    }
}
