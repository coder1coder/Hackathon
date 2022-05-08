using System;
using System.Threading.Tasks;
using FluentAssertions;
using FluentValidation;
using Hackathon.Abstraction.Event;
using Hackathon.Abstraction.Project;
using Hackathon.Abstraction.Team;
using Hackathon.BL.Team;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Team;
using Moq;
using Xunit;

namespace Hackathon.BL.Tests.Team;

public class TeamServiceTests: BaseUnitTest
{
    private Mock<IValidator<CreateTeamModel>> _createTeamModelValidatorMock = new();
    private Mock<ITeamRepository> _teamRepositoryMock = new();
    private Mock<IValidator<TeamMemberModel>> _teamAddMemberValidatorMock;
    private Mock<IValidator<GetListParameters<TeamFilter>>> _getFilterModelValidatorMock;
    private Mock<IEventRepository> _eventRepositoryMock = new();
    private Mock<IProjectRepository> _projectRepositoryMock = new();
    
    [Fact]
    public async Task Create_Should_Return_Positive_Id()
    {
        //arrange
        var createdId = new Random().Next(0, 11);
        
        _createTeamModelValidatorMock = new Mock<IValidator<CreateTeamModel>>();
        _teamRepositoryMock = new Mock<ITeamRepository>();
        _eventRepositoryMock = new Mock<IEventRepository>();
        _projectRepositoryMock = new Mock<IProjectRepository>();
        _teamAddMemberValidatorMock = new Mock<IValidator<TeamMemberModel>>();
        _getFilterModelValidatorMock = new Mock<IValidator<GetListParameters<TeamFilter>>>();
       
       _teamRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<CreateTeamModel>()))
            .ReturnsAsync(createdId);

        var service = new TeamService(
            _createTeamModelValidatorMock.Object,
            _teamAddMemberValidatorMock.Object,
            _getFilterModelValidatorMock.Object,
            _teamRepositoryMock.Object,
            _eventRepositoryMock.Object,
            _projectRepositoryMock.Object);

        //act
        var result = await service.CreateAsync(new CreateTeamModel());
        //assert
        result.Should().Be(createdId);
    }
}