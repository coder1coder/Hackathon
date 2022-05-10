using System;
using System.Threading.Tasks;
using FluentAssertions;
using FluentValidation;
using Hackathon.Abstraction.Notification;
using Hackathon.Abstraction.Project;
using Hackathon.Abstraction.Team;
using Hackathon.Abstraction.User;
using Hackathon.BL.Project;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Project;
using MassTransit;
using Moq;
using Xunit;

namespace Hackathon.BL.Tests.Project;

public class ProjectServiceTests: BaseUnitTest
{
    private Mock<IValidator<ProjectCreateModel>> _createValidatorMock;
    private Mock<IProjectRepository> _projectRepositoryMock;

    public ProjectServiceTests()
    {
        _createValidatorMock = new Mock<IValidator<ProjectCreateModel>>();
        _projectRepositoryMock = new Mock<IProjectRepository>();
    }
    
    [Fact]
    public async Task Create_Should_Return_Positive_Id()
    {
        //arrange
        var createdId = new Random().Next(0, 11);
        
        _projectRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<ProjectCreateModel>()))
            .ReturnsAsync(createdId);

        var service = new ProjectService(
            _projectRepositoryMock.Object,
            _createValidatorMock.Object
        );

        //act
        var result = await service.CreateAsync(new ProjectCreateModel());
        //assert
        result.Should().Be(createdId);
    }
}