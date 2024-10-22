using System.Linq;
using System.Threading.Tasks;
using Bogus;
using FluentValidation;
using Hackathon.BL.Users;
using Hackathon.Common.Abstraction.User;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Users;
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
    private readonly Mock<IFileStorageService> _fileStorageMock = new();
    private readonly Mock<IValidator<CreateNewUserModel>> _signUpValidatorMock = new();
    private readonly Mock<IValidator<UpdateUserParameters>> _updateUserParametersValidator = new();
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly Mock<IOptions<EmailSettings>> _emailSettingsMock = new();
    private readonly Mock<IEmailConfirmationRepository> _emailConfirmationRepositoryMock = new();
    private readonly Mock<IFileStorageRepository> _fileStorageRepositoryMock = new();
    private readonly Mock<IValidator<IFileImage>> _profileImageValidator = new();
    private readonly Mock<IValidator<UpdatePasswordModel>> _updatePasswordModelValidatorMock = new();
    private readonly Mock<IPasswordHashService> _passwordHashServiceMock = new();

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
            _profileImageValidator.Object,
            _signUpValidatorMock.Object,
            _updateUserParametersValidator.Object,
            _userRepositoryMock.Object,
            _emailConfirmationRepositoryMock.Object,
            _fileStorageMock.Object,
            _fileStorageRepositoryMock.Object,
            _updatePasswordModelValidatorMock.Object,
            _passwordHashServiceMock.Object);

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
