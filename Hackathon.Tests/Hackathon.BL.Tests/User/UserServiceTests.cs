using System.Linq;
using System.Threading.Tasks;
using Bogus;
using FluentValidation;
using Hackathon.Abstraction.FileStorage;
using Hackathon.Abstraction.User;
using Hackathon.BL.User;
using Hackathon.Common.Configuration;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.User;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace Hackathon.BL.Tests.User;

public class UserServiceTests: BaseUnitTest
{
    private readonly Mock<IFileStorageService> _fileStorageMock;
    private readonly Mock<IOptions<AppSettings>> _appSettingsMock;
    private readonly Mock<IValidator<SignUpModel>> _signUpValidatorMock;
    private readonly Mock<IValidator<SignInModel>> _signInValidatorMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;

    public UserServiceTests()
    {
        _fileStorageMock = new Mock<IFileStorageService>();
        _appSettingsMock = new Mock<IOptions<AppSettings>>();
        _signUpValidatorMock = new Mock<IValidator<SignUpModel>>();
        _signInValidatorMock = new Mock<IValidator<SignInModel>>();
        _userRepositoryMock = new Mock<IUserRepository>();
    }

    [Fact]
    public async Task GetAsync_ShouldReturn_UserModelCollection()
    {
        //arrange
        var fakeUser = new Faker<UserModel>()
            .RuleFor(x => x.UserName, f => f.Name.FirstName())
            .RuleFor(x => x.Email, f => new UserEmailModel
            {
                Address = f.Internet.Email()
            })
            .Generate();

        _userRepositoryMock.Setup(x => x.GetAsync(It.IsAny<GetListParameters<UserFilter>>()))
            .ReturnsAsync(new BaseCollection<UserModel>
            {
                Items = new[] {fakeUser},
                TotalCount = 1
            });

        var sut = new UserService(
            _appSettingsMock.Object,
            _signUpValidatorMock.Object,
            _signInValidatorMock.Object,
            _userRepositoryMock.Object,
            _fileStorageMock.Object,
            Mapper
            );

        //act
        var result = await sut.GetAsync(new GetListParameters<UserFilter>
        {
            Filter = new UserFilter
            {
                Username = fakeUser.UserName,
                Email = fakeUser.Email.Address
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
