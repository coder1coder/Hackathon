using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Hackathon.Common.Models.Team;
using Hackathon.Tests.Integration.Base;
using Xunit;

namespace Hackathon.Tests.Integration.Team
{
    public class TeamRepositoryTests: BaseIntegrationTest
    {
        public TeamRepositoryTests(TestWebApplicationFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task ExistAsync_ById_ShouldReturn_True()
        {
            var (team, _) = await CreateTeamWithEvent(TestUser.Id);
            var isExist = await TeamRepository.ExistAsync(team.Id);
            isExist.Should().BeTrue();
        }

        [Fact]
        public async Task ExistAsync_ByName_ShouldReturn_Success()
        {
            var (team, _) = await CreateTeamWithEvent(TestUser.Id);
            var isExist = await TeamRepository.ExistAsync(team.Name);
            isExist.Should().BeTrue();
        }

        [Fact]
        public async Task CreateAsync_ShouldReturn_Id()
        {
            var (team, _) = await CreateTeamWithEvent(TestUser.Id);
            team.Id.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task GetAsync_ShouldReturn_NotNull()
        {
            var (team, _) = await CreateTeamWithEvent(TestUser.Id);
            var teamModel = await TeamRepository.GetAsync(team.Id);
            teamModel.Should().NotBeNull();
        }

        [Fact]
        public async Task AddMemberAsync_ShouldReturn_Success()
        {
            var (team, _) = await CreateTeamWithEvent(TestUser.Id);

            var signUpModel = TestFaker.GetSignUpModels(1).First();
            var createdUser = await UserRepository.CreateAsync(signUpModel);

            await FluentActions
                .Invoking(async () => await TeamRepository.AddMemberAsync(new TeamMemberModel
                {
                    TeamId = team.Id,
                    MemberId = createdUser.Id
                }))
                .Should()
                .NotThrowAsync();
        }
    }
}