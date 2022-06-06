using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Hackathon.Common.Models;
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

            long userId = -1;
            
            UserRepository.CreateAsync(signUpModel)
                .ContinueWith(x=>
                    userId = x.Result)
                .Wait();

            await FluentActions
                .Invoking(async () => await TeamRepository.AddMemberAsync(new TeamMemberModel
                {
                    TeamId = team.Id,
                    MemberId = userId
                }))
                .Should()
                .NotThrowAsync();
        }

        [Fact]
        public async Task GetAsync_WithGlobalFilter_ShouldReturn_Teams_Where_IsDeletedFalse()
        {
            const int validTeamsQuantity = 3;
            var teamEntities = TestFaker.GetTeamEntities(10).ToList();

            for (var i = 0; i < teamEntities.Count - validTeamsQuantity; i++)
            {
                teamEntities[i].IsDeleted = true;
            }

            await DbContext.Teams.AddRangeAsync(teamEntities);
            await DbContext.SaveChangesAsync();

            var response = await TeamRepository.GetAsync(new GetListParameters<TeamFilter>
            {
                Limit = int.MaxValue
            });

            var createdTeamEntities = teamEntities.Where(x => response.Items.Any(f => f.Id == x.Id)).ToArray();

            createdTeamEntities.Should().NotBeEmpty();
            createdTeamEntities.Should().HaveCount(validTeamsQuantity);
            createdTeamEntities.Any(x => x.IsDeleted).Should().BeFalse();
            response.Items
                .First(x => x.Id == createdTeamEntities.First().Id)
                .Should()
                .BeEquivalentTo(createdTeamEntities.First(), options =>
                    options
                        .Excluding(x => x.Events)
                        .Excluding(x => x.Members)
                        .Excluding(x => x.IsDeleted)
                        .Excluding(x => x.Owner)
                );
        }
    }
}