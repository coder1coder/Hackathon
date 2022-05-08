using System;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using FluentValidation;
using Hackathon.Abstraction.Event;
using Hackathon.Abstraction.Project;
using Hackathon.Abstraction.Team;
using Hackathon.BL.Team;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Team;
using Moq;
using Xunit;

namespace Hackathon.BL.Tests.Team;

public class TeamServiceTests: BaseUnitTest
{
    private Mock<IValidator<CreateTeamModel>> _createTeamModelValidatorMock;
    private Mock<ITeamRepository> _teamRepositoryMock;
    private Mock<IValidator<TeamMemberModel>> _teamAddMemberValidatorMock;
    private Mock<IValidator<GetListParameters<TeamFilter>>> _getFilterModelValidatorMock;
    private Mock<IEventRepository> _eventRepositoryMock;
    private Mock<IProjectRepository> _projectRepositoryMock;

    public TeamServiceTests()
    {
        _createTeamModelValidatorMock = new Mock<IValidator<CreateTeamModel>>();
        _teamRepositoryMock = new Mock<ITeamRepository>();
        _eventRepositoryMock = new Mock<IEventRepository>();
        _projectRepositoryMock = new Mock<IProjectRepository>();
        _teamAddMemberValidatorMock = new Mock<IValidator<TeamMemberModel>>();
        _getFilterModelValidatorMock = new Mock<IValidator<GetListParameters<TeamFilter>>>();
    }
    
    [Fact]
    public async Task Create_Should_Return_Positive_Id()
    {
        //arrange
        var createdId = new Random().Next(0, 11);
        
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
    
    [Fact]
    public async Task GetAsync_ShouldReturn_TeamModelCollection()
    {
        //arrange
        var fakeTeam = new Faker<TeamModel>()
            .RuleFor(x => x.Name, f => f.Name.Suffix())
            .RuleFor(x => x.OwnerId, f => f.PickRandom(0,11))
            .Generate();
        
        _teamRepositoryMock.Setup(x => 
                x.GetAsync(It.IsAny<GetListParameters<TeamFilter>>()))
            .ReturnsAsync(new BaseCollection<TeamModel>
            {
                Items = new[] {fakeTeam},
                TotalCount = 1
            });
        
        var service = new TeamService(
            _createTeamModelValidatorMock.Object,
            _teamAddMemberValidatorMock.Object,
            _getFilterModelValidatorMock.Object,
            _teamRepositoryMock.Object,
            _eventRepositoryMock.Object,
            _projectRepositoryMock.Object);
        
        //act
        var result = await service.GetAsync(new GetListParameters<TeamFilter>()
        {
            Filter = new TeamFilter
            {
                Name = fakeTeam.Name,
                OwnerId = fakeTeam.OwnerId
            }
        });

        //assert
        Assert.NotNull(result);
        Assert.Equal(1, result.TotalCount);
        Assert.Equal(result.TotalCount, result.Items.Count);

        var first = result.Items.First();
        Assert.Equal(fakeTeam.Name, first.Name);
        Assert.Equal(fakeTeam.OwnerId, first.OwnerId);
    }
}