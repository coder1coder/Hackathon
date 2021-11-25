using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Hackathon.Common.Abstraction;
using Hackathon.Common.Models.Team;
using Hackathon.DAL.Repositories;
using Hackathon.Tests.Common;
using Mapster;
using Xunit;

namespace Hackathon.Tests.Unit
{
    public class TeamRepositoryTests: BaseUnitTests
    {
        private readonly ITeamRepository _teamRepository;
        private readonly IUserRepository _userRepository;
        private readonly EventRepository _eventRepository;

        public TeamRepositoryTests()
        {
            _teamRepository = new TeamRepository(Mapper, DbContext);
            _userRepository = new UserRepository(Mapper, DbContext);
            _eventRepository = new EventRepository(Mapper, DbContext);
        }

        [Fact]
        public async Task ExistAsync_ById_ShouldReturn_True()
        {
            var createdTeamModel = await CreateTeamWithEvent();
            var isExist = await _teamRepository.ExistAsync(createdTeamModel.Id);
            isExist.Should().BeTrue();
        }

        [Fact]
        public async Task ExistAsync_ByName_ShouldReturn_Success()
        {
            var teamModel = await CreateTeamWithEvent();
            var isExist = await _teamRepository.ExistAsync(teamModel.Name);
            isExist.Should().BeTrue();
        }

        [Fact]
        public async Task CreateAsync_ShouldReturn_Id()
        {
            var createdTeamModel = await CreateTeamWithEvent();
            createdTeamModel.Id.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task GetAsync_ShouldReturn_NotNull()
        {
            var createdTeamModel = CreateTeamWithEvent();
            var teamModel = await _teamRepository.GetAsync(createdTeamModel.Id);
            teamModel.Should().NotBeNull();
        }

        [Fact]
        public async Task AddMemberAsync_ShouldReturn_Success()
        {
            var createdTeamModel = await CreateTeamWithEvent();

            var signUpModel = TestFaker.GetSignUpModels(1).First();
            var createdUserId = await _userRepository.CreateAsync(signUpModel);

            await FluentActions
                .Invoking(async () => await _teamRepository.AddMemberAsync(new TeamAddMemberModel()
                {
                    TeamId = createdTeamModel.Id,
                    UserId = createdUserId
                }))
                .Should()
                .NotThrowAsync();
        }

        private async Task<TeamModel> CreateTeamWithEvent()
        {
            var eventModel = TestFaker.GetCreateEventModels(1).First();
            var eventId = await _eventRepository.CreateAsync(eventModel);

            var createTeamModel = TestFaker.GetCreateTeamModels(1).First();
            createTeamModel.EventId = eventId;

            var teamModel = createTeamModel.Adapt<TeamModel>();

            teamModel.Id = await _teamRepository.CreateAsync(createTeamModel);
            return teamModel;
        }
    }
}