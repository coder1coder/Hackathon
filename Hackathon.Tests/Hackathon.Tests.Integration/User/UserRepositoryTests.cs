using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Hackathon.Common.Models;
using Hackathon.Common.Models.User;
using Hackathon.Tests.Integration.Base;
using Xunit;

namespace Hackathon.Tests.Integration.User
{
    public class UserRepositoryTests: BaseIntegrationTest
    {
        public UserRepositoryTests(TestWebApplicationFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task CreateAsync_ShouldReturn_Id()
        {
            var signUpModel = TestFaker.GetSignUpModels(1).First();
            var userId = await UserRepository.CreateAsync(signUpModel);
            userId.Should().BeGreaterThan(default);
        }

        [Fact]
        public async Task GetAsync_ShouldReturn_Success()
        {
            var signUpModel = TestFaker.GetSignUpModels(1).First();
            var userId = await UserRepository.CreateAsync(signUpModel);
            var userModel = await UserRepository.GetAsync(userId);
            userModel.Should().BeEquivalentTo(signUpModel, options=>
                options.Excluding(x=>x.Password)
                );
        }

        [Fact]
        public async Task GetAsync_WithGetFilterModel_ShouldReturn_Success()
        {
            var userEntities = TestFaker.GetUserEntities(5).ToArray();

            await DbContext.Users.AddRangeAsync(userEntities);
            await DbContext.SaveChangesAsync();

            var response = await UserRepository.GetAsync(new GetListParameters<UserFilter>
            {
                Filter = new UserFilter
                {
                    Username = userEntities.First().UserName,
                    Email = userEntities.First().Email
                }
            });

            response.TotalCount.Should().Be(1);
            response.Items
                .First()
                .Should()
                .BeEquivalentTo(userEntities.First(), options=>
                    options
                        .Excluding(x=>x.Teams)
                        .Excluding(x=>x.GoogleAccountId)
                        .Excluding(x => x.IsDeleted)
                    );
        }

        [Fact]
        public async Task ExistAsync_ShouldReturn_Success()
        {
            var signUpModel = TestFaker.GetSignUpModels(1).First();
            var userId = await UserRepository.CreateAsync(signUpModel);

            var exist = await UserRepository.ExistAsync(userId);
            exist.Should().BeTrue();
        }

        [Fact]
        public async Task GetAsync_WithGlobalFilter_ShouldReturn_Users_Where_IsDeletedFalse()
        {
            const int validUsersQuantity = 3;
            var userEntities = TestFaker.GetUserEntities(10).ToList();

            for (var i = 0; i < userEntities.Count - validUsersQuantity; i++)
            {
                userEntities[i].IsDeleted = true;
            }

            await DbContext.Users.AddRangeAsync(userEntities);
            await DbContext.SaveChangesAsync();

            var response = await UserRepository.GetAsync(new GetListParameters<UserFilter>
            {
                Limit = int.MaxValue
            });

            var createdUserEntities = userEntities.Where(x => response.Items.Any(f => f.Id == x.Id)).ToArray();

            createdUserEntities.Should().NotBeEmpty();
            createdUserEntities.Should().HaveCount(validUsersQuantity);
            createdUserEntities.Any(x => x.IsDeleted).Should().BeFalse();
            response.Items
                .First(x => x.Id == createdUserEntities.First().Id)
                .Should()
                .BeEquivalentTo(createdUserEntities.First(), options =>
                    options
                        .Excluding(x => x.Teams)
                        .Excluding(x => x.GoogleAccountId)
                        .Excluding(x => x.IsDeleted)
                );
        }
    }
}