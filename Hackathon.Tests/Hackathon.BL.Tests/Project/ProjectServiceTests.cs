using BackendTools.Common.Models;
using System.Threading.Tasks;
using FluentValidation;
using Hackathon.BL.Projects;
using Hackathon.BL.Validation.Projects;
using Hackathon.Common.Abstraction.Project;
using Hackathon.Common.Models.Project;
using Moq;
using Xunit;

namespace Hackathon.BL.Tests.Project;

public class ProjectServiceTests: BaseUnitTest
{
    private readonly IValidator<UpdateProjectFromGitBranchParameters> _updateFromGitBranchValidator;
    private readonly Mock<Hackathon.Common.Abstraction.IValidator<ProjectCreationParameters>> _createValidatorMock;
    private readonly Mock<Hackathon.Common.Abstraction.IValidator<ProjectUpdateParameters>> _updateValidatorMock;
    private readonly Mock<IValidator<UpdateProjectFromGitBranchParameters>> _updateFromGitBranchValidatorMock;
    private readonly Mock<IProjectRepository> _projectRepositoryMock;

    public ProjectServiceTests()
    {
        IValidator<IHasProjectIdentity> iHasProjectIdentityValidator = new ProjectIdentityParametersValidator();
        _updateFromGitBranchValidator = new UpdateProjectFromGitParametersValidator(iHasProjectIdentityValidator);
        
        _updateFromGitBranchValidatorMock = new Mock<IValidator<UpdateProjectFromGitBranchParameters>>();
        _createValidatorMock = new Mock<Hackathon.Common.Abstraction.IValidator<ProjectCreationParameters>>();
        _updateValidatorMock = new Mock<Hackathon.Common.Abstraction.IValidator<ProjectUpdateParameters>>();
        _projectRepositoryMock = new Mock<IProjectRepository>();
    }

    [Fact]
    public async Task Create_Should_Return_Positive_Id()
    {
        //arrange
        _projectRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<ProjectCreationParameters>()))
            .Returns(Task.CompletedTask);

        var service = new ProjectService(
            _projectRepositoryMock.Object,
            _createValidatorMock.Object,
            _updateValidatorMock.Object,
            _updateFromGitBranchValidatorMock.Object,
            null,
            null
        );

        _createValidatorMock.Setup(x => x.ValidateAsync(It.IsAny<ProjectCreationParameters>(), It.IsAny<long>()))
            .ReturnsAsync(Result.Success);

        //act
        var result = await service.CreateAsync(default, new ProjectCreationParameters());

        //assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
    }

    [Theory]
    [InlineData("https://github.com/as_as-BR.0912/project_01.ASD-A/tree/main")]
    [InlineData("https://github.com/ASDasd0099/ASDasd__0001--2s/tree/main/")]
    [InlineData("https://github.com/Proger0014/url-shortener/tree/main")]
    [InlineData("https://github.com/Asd.a9/Asd.a9_a/tree/main/")]
    public async Task ValidateBranchLinks_SuiteValidBranchLinks_ReturnTrue(string branchLink)
    {
        // arrange
        var value = new UpdateProjectFromGitBranchParameters()
        {
            LinkToGitBranch = branchLink,
            EventId = 1,
            TeamId = 1
        };
        
        // act
        var result = await _updateFromGitBranchValidator.ValidateAsync(value);

        // assert
        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData("https://github.com/as/_as-BR.0912/project_01.ASD-A/tree/main")]
    [InlineData("https://github.com/ASDasd0099/ASDasd__0001--2s/tree/main/directory")]
    [InlineData("https://github.com/Proger0014/url-shortenertree/main")]
    public async Task ValidateBranchLinks_SuiteUnvalidBranchLinks_ReturnFalse(string branchLink)
    {
        // arrange
        var value = new UpdateProjectFromGitBranchParameters()
        {
            LinkToGitBranch = branchLink,
            EventId = 1,
            TeamId = 1
        };
        
        // act
        var result = await _updateFromGitBranchValidator.ValidateAsync(value);

        // assert
        Assert.False(result.IsValid);
    }
}
