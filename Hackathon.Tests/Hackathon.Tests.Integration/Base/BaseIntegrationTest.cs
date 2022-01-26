using System;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Hackathon.API.Abstraction;
using Hackathon.Common.Abstraction;
using Hackathon.Common.Configuration;
using Hackathon.Common.Models.Team;
using Hackathon.Contracts.Requests.User;
using Hackathon.DAL;
using Hackathon.DAL.Mappings;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Refit;

namespace Hackathon.Tests.Integration.Base
{
    public class BaseIntegrationTest
    {
        protected readonly IAuthApi AuthApi;
        protected readonly IUserApi UsersApi;
        protected readonly IEventApi EventsApi;
        protected readonly ITeamApi TeamsApi;
        protected readonly IProjectApi ProjectsApi;

        protected readonly IUserRepository UserRepository;
        protected readonly IEventRepository EventRepository;
        protected readonly ITeamRepository TeamRepository;
        protected readonly IProjectRepository ProjectRepository;
        protected readonly IMapper Mapper;

        protected readonly ApplicationDbContext DbContext;
        protected readonly TestFaker TestFaker;

        protected readonly AdministratorDefaults AdministratorDefaultsConfig;

        protected long UserId { get; set; }

        protected BaseIntegrationTest(TestWebApplicationFactory factory)
        {
            AdministratorDefaultsConfig = factory.Services.GetRequiredService<IOptions<AdministratorDefaults>>().Value;

            DbContext = factory.Services.GetRequiredService<ApplicationDbContext>();

            UserRepository = factory.Services.GetRequiredService<IUserRepository>();
            EventRepository = factory.Services.GetRequiredService<IEventRepository>();
            TeamRepository = factory.Services.GetRequiredService<ITeamRepository>();
            ProjectRepository = factory.Services.GetRequiredService<IProjectRepository>();

            var userService = factory.Services.GetRequiredService<IUserService>();

            var config = new TypeAdapterConfig();
            config.Apply(new EventEntityMapping(), new TeamEntityMapping(), new UserEntityMapping());
            Mapper = new Mapper(config);
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
            UsersApi = RestService.For<IUserApi>(httpClient);
            EventsApi = RestService.For<IEventApi>(httpClient);
            TeamsApi = RestService.For<ITeamApi>(httpClient);
            ProjectsApi = RestService.For<IProjectApi>(httpClient);

            InitialData().GetAwaiter().GetResult();
        }

        private async Task InitialData()
        {
            var fakeRequest = Mapper.Map<SignUpRequest>(TestFaker.GetSignUpModels(1).First());
            var response = await UsersApi.SignUp(fakeRequest);

            UserId = response.Id;
        }

        protected async Task<TeamModel> CreateTeamWithEvent(long userId)
        {
            var eventModel = TestFaker.GetCreateEventModels(1, userId).First();
            var eventId = await EventRepository.CreateAsync(eventModel);

            var createTeamModel = TestFaker.GetCreateTeamModels(1).First();
            createTeamModel.EventId = eventId;

            var teamModel = Mapper.Map<TeamModel>(createTeamModel);

            teamModel.Id = await TeamRepository.CreateAsync(createTeamModel);
            return teamModel;
        }
    }
}