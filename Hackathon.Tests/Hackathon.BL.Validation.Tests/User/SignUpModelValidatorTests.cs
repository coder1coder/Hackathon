using FluentValidation.TestHelper;
using Hackathon.BL.Validation.Users;
using Hackathon.Common.Abstraction.User;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.User;
using Hackathon.Configuration;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace Hackathon.BL.Validation.Tests.User;

public class SignUpModelValidatorTests
{
    private readonly SignUpModelValidator _validator;

    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly RestrictedNames _restrictedNames;

    public SignUpModelValidatorTests()
    {
        _restrictedNames = new RestrictedNames
        {
            Users = new[] {"administrator", "admin"}
        };

        _validator = new SignUpModelValidator(_userRepositoryMock.Object, new OptionsWrapper<RestrictedNames>(_restrictedNames));
    }

    [Fact]
    public void Should_Fail_When_Specify_UserName_From_ListOfRestrictions()
    {
        //arrange
        var signUpModel = new SignUpModel
        {
            Email = "test@test.ru",
            UserName = _restrictedNames.Users.First()
        };

        _userRepositoryMock.Setup(x => x
                .GetAsync(It.Is<GetListParameters<UserFilter>>(s =>
                    s.Filter != null && s.Filter.Email == signUpModel.Email)))
            .ReturnsAsync(new BaseCollection<UserModel>
            {
                Items = ArraySegment<UserModel>.Empty,
                TotalCount = 0
            });

        //act
        //assert
        _validator.TestValidate(signUpModel)
            .ShouldHaveValidationErrorFor(x => x.UserName)
            .WithErrorMessage(SignUpModelValidator.SpecifyUserNameIsRestricted);
    }


}
