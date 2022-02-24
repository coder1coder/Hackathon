using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Hackathon.Common.Models.Team;
using Hackathon.Tests.Integration.Base;
using Xunit;

namespace Hackathon.Tests.Integration.Team
{
    public class TeamRepositoryTests: BaseIntegrationTest, IClassFixture<TestWebApplicationFactory>
    {
        public TeamRepositoryTests(TestWebApplicationFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task ExistAsync_ById_ShouldReturn_True()
        {
            var createdTeamModel = await CreateTeamWithEvent(UserId);
            var isExist = await TeamRepository.ExistAsync(createdTeamModel.Id);
            isExist.Should().BeTrue();
        }

        [Fact]
        public async Task ExistAsync_ByName_ShouldReturn_Success()
        {
            var teamModel = await CreateTeamWithEvent(UserId);
            var isExist = await TeamRepository.ExistAsync(teamModel.Name);
            isExist.Should().BeTrue();
        }

        [Fact]
        public async Task CreateAsync_ShouldReturn_Id()
        {
            var createdTeamModel = await CreateTeamWithEvent(UserId);
            createdTeamModel.Id.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task GetAsync_ShouldReturn_NotNull()
        {
            var createdTeamModel = await CreateTeamWithEvent(UserId);
            var teamModel = await TeamRepository.GetAsync(createdTeamModel.Id);
            teamModel.Should().NotBeNull();
        }

        [Fact]
        public async Task AddMemberAsync_ShouldReturn_Success()
        {
            var createdTeamModel = await CreateTeamWithEvent(UserId);

            var signUpModel = TestFaker.GetSignUpModels(1).First();
            var createdUser = await UserRepository.CreateAsync(signUpModel);

            await FluentActions
                .Invoking(async () => await TeamRepository.AddMemberAsync(new TeamMemberModel()
                {
                    TeamId = createdTeamModel.Id,
                    UserId = createdUser.Id
                }))
                .Should()
                .NotThrowAsync();
        }
    }
}