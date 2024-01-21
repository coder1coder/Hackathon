using System.Linq;
using System.Threading.Tasks;
using Bogus;
using FluentValidation;
using Hackathon.BL.Users;
using Hackathon.Common.Abstraction.User;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.User;
using Hackathon.Configuration;
using Hackathon.FileStorage.Abstraction.Models;
using Hackathon.FileStorage.Abstraction.Repositories;
using Hackathon.FileStorage.Abstraction.Services;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace Hackathon.BL.Tests.User;

public class UserServiceTests: BaseUnitTest
{
    private readonly Mock<IFileStorageService> _fileStorageMock;
    private readonly Mock<IValidator<SignUpModel>> _signUpValidatorMock;
    private readonly Mock<IValidator<SignInModel>> _signInValidatorMock;
    private readonly Mock<IValidator<UpdateUserParameters>> _updateUserParametersValidator;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IOptions<EmailSettings>> _emailSettingsMock;
    private readonly Mock<IOptions<AuthOptions>> _authOptionsMock;
    private readonly Mock<IEmailConfirmationRepository> _emailConfirmationRepositoryMock;
    private readonly Mock<IFileStorageRepository> _fileStorageRepositoryMock;
    private readonly Mock<IValidator<IFileImage>> _profileImageValidator;

    public UserServiceTests()
    {
        _updateUserParametersValidator = new Mock<IValidator<UpdateUserParameters>>();
        _fileStorageMock = new Mock<IFileStorageService>();
        _emailSettingsMock = new Mock<IOptions<EmailSettings>>();
        _authOptionsMock = new Mock<IOptions<AuthOptions>>();
        _signUpValidatorMock = new Mock<IValidator<SignUpModel>>();
        _signInValidatorMock = new Mock<IValidator<SignInModel>>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _emailConfirmationRepositoryMock = new Mock<IEmailConfirmationRepository>();
        _fileStorageRepositoryMock = new Mock<IFileStorageRepository>();
        _profileImageValidator = new Mock<IValidator<IFileImage>>();
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
            _emailSettingsMock.Object,
            _authOptionsMock.Object,
            _profileImageValidator.Object,
            _signUpValidatorMock.Object,
            _signInValidatorMock.Object,
            _updateUserParametersValidator.Object,
            _userRepositoryMock.Object,
            _emailConfirmationRepositoryMock.Object,
            _fileStorageMock.Object,
            _fileStorageRepositoryMock.Object,
            Mapper
            );

        //act
        var result = await sut.GetListAsync(new GetListParameters<UserFilter>
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
