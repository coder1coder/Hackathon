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
    
    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(9)]
    public async Task GetAsync_ShouldReturn_Success(int teamCount)
    {
        //arrange
        var fakeTeams = new Faker<TeamModel>()
            .RuleFor(x => x.Id, f => f.PickRandom(0,1001))
            .Generate(teamCount).ToArray();
        
        _teamRepositoryMock.Setup(x => 
                x.GetAsync(It.Is<GetListParameters<TeamFilter>>(
                    x=>x.Filter.Ids.Contains(fakeTeams.First().Id)
                    && x.Filter.Ids.Length == 1 )))
            .ReturnsAsync(new BaseCollection<TeamModel>
            {
                Items = new []{fakeTeams.First()},
                TotalCount = fakeTeams.Length
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
                Ids = new [] {fakeTeams.First().Id}
            }
        });

        //assert
        Assert.NotNull(result);
        Assert.Equal(fakeTeams.Length, result.TotalCount);
        Assert.Equal(1, result.Items.Count);

        var first = result.Items.First();
        Assert.Equal(fakeTeams.First().Id, first.Id);
    }
}