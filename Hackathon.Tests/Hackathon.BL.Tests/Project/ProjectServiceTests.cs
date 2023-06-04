using BackendTools.Common.Models;
using System.Threading.Tasks;
using FluentValidation;
using Hackathon.BL.Project;
using Hackathon.Common.Abstraction.Project;
using Hackathon.Common.Models.Project;
using Moq;
using Xunit;

namespace Hackathon.BL.Tests.Project;

public class ProjectServiceTests: BaseUnitTest
{
    private readonly Mock<Hackathon.Common.Abstraction.IValidator<ProjectCreationParameters>> _createValidatorMock;
    private readonly Mock<Hackathon.Common.Abstraction.IValidator<ProjectUpdateParameters>> _updateValidatorMock;
    private readonly Mock<IValidator<UpdateProjectFromGitBranchParameters>> _updateFromGitBranchValidator;
    private readonly Mock<IProjectRepository> _projectRepositoryMock;

    public ProjectServiceTests()
    {
        _updateFromGitBranchValidator = new Mock<IValidator<UpdateProjectFromGitBranchParameters>>();
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
            _updateFromGitBranchValidator.Object,
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
}
