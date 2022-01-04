using System;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Hackathon.API.Abstraction;
using Hackathon.API.Client.Event;
using Hackathon.API.Client.Project;
using Hackathon.API.Client.Team;
using Hackathon.API.Client.User;
using Hackathon.Common.Abstraction;
using Hackathon.Common.Models.Team;
using Hackathon.DAL;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace Hackathon.Tests.Integration.Base
{
    public class BaseIntegrationTest
    {
        protected readonly IAuthApi AuthApi;
        protected readonly IUserApiClient UsersApi;
        protected readonly IEventApiClient EventsApi;
        protected readonly ITeamApiClient TeamsApi;
        protected readonly IProjectApiClient ProjectsApi;

        protected readonly IUserRepository UserRepository;
        protected readonly IEventRepository EventRepository;
        protected readonly ITeamRepository TeamRepository;
        protected readonly IProjectRepository ProjectRepository;
        protected readonly IMapper Mapper;

        protected readonly ApplicationDbContext DbContext;
        protected readonly TestFaker TestFaker;

        protected BaseIntegrationTest(TestWebApplicationFactory factory)
        {
            DbContext = factory.Services.GetRequiredService<ApplicationDbContext>();
            UserRepository = factory.Services.GetRequiredService<IUserRepository>();
            EventRepository = factory.Services.GetRequiredService<IEventRepository>();
            TeamRepository = factory.Services.GetRequiredService<ITeamRepository>();
            ProjectRepository = factory.Services.GetRequiredService<IProjectRepository>();

            var userService = factory.Services.GetRequiredService<IUserService>();

            Mapper = new Mapper();
            TestFaker = new TestFaker(Mapper);

            var httpClient = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
                HandleCookies = false
            });

            var authToken = userService.GenerateToken(1);
            httpClient.BaseAddress = new Uri("https://localhost:7001/");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken.Token);

            AuthApi = RestService.For<IAuthApi>(httpClient);
            UsersApi = RestService.For<IUserApiClient>(httpClient);
            EventsApi = RestService.For<IEventApiClient>(httpClient);
            TeamsApi = RestService.For<ITeamApiClient>(httpClient);
            ProjectsApi = RestService.For<IProjectApiClient>(httpClient);
        }

        protected async Task<TeamModel> CreateTeamWithEvent()
        {
            var eventModel = TestFaker.GetCreateEventModels(1).First();
            var eventId = await EventRepository.CreateAsync(eventModel);

            var createTeamModel = TestFaker.GetCreateTeamModels(1).First();
            createTeamModel.EventId = eventId;

            var teamModel = Mapper.Map<TeamModel>(createTeamModel);

            teamModel.Id = await TeamRepository.CreateAsync(createTeamModel);
            return teamModel;
        }
    }
}