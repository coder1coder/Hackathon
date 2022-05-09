using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Hackathon.Tests.Integration.Base;
using Xunit;

namespace Hackathon.Tests.Integration.Project
{
    public class ProjectRepositoryTests: BaseIntegrationTest
    {
        public ProjectRepositoryTests(TestWebApplicationFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task CreateAsync_ShouldReturn_Id()
        {
            var (teamModel, eventId) = await CreateTeamWithEvent(TestUserId);
            var projectCreateModel = TestFaker.GetProjectCreateModel(1).First();
            projectCreateModel.TeamId = teamModel.Id;
            projectCreateModel.EventId = eventId;

            var createdProjectId = await ProjectRepository.CreateAsync(projectCreateModel);
            createdProjectId.Should().BeGreaterThan(0);
        }
    }
}