using System;
using System.Threading.Tasks;
using FluentAssertions;
using FluentValidation;
using Hackathon.BL.Project;
using Hackathon.Common.Abstraction.Project;
using Hackathon.Common.Models.Project;
using Moq;
using Xunit;

namespace Hackathon.BL.Tests.Project;

public class ProjectServiceTests: BaseUnitTest
{
    private readonly Mock<IValidator<ProjectCreateParameters>> _createValidatorMock;
    private readonly  Mock<IProjectRepository> _projectRepositoryMock;

    public ProjectServiceTests()
    {
        _createValidatorMock = new Mock<IValidator<ProjectCreateParameters>>();
        _projectRepositoryMock = new Mock<IProjectRepository>();
    }

    [Fact]
    public async Task Create_Should_Return_Positive_Id()
    {
        //arrange
        var createdId = new Random().Next(0, 11);

        _projectRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<ProjectCreateParameters>()))
            .ReturnsAsync(createdId);

        var service = new ProjectService(
            _projectRepositoryMock.Object,
            _createValidatorMock.Object,
            null,
            null,
            null
        );

        //act
        var result = await service.CreateAsync(new ProjectCreateParameters());

        //assert
        result.Should().Be(createdId);
    }
}
