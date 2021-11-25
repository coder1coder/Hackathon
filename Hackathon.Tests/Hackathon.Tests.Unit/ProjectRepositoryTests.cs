using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Hackathon.Tests.Common;
using Xunit;

namespace Hackathon.Tests.Unit
{
    public class ProjectRepositoryTests: BaseUnitTests
    {
        [Fact]
        public async Task CreateAsync_ShouldReturn_Id()
        {
            var teamModel = await CreateTeamWithEvent();
            var projectCreateModel = TestFaker.GetProjectCreateModel(1).First();
            projectCreateModel.TeamId = teamModel.Id;

            var createdProjectId = await ProjectRepository.CreateAsync(projectCreateModel);
            createdProjectId.Should().BeGreaterThan(0);
        }
    }
}