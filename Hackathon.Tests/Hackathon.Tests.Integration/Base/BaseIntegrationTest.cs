using System;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Hackathon.API.Client;
using Hackathon.Common.Abstraction;
using Hackathon.Common.Models.Team;
using Hackathon.DAL;
using Hackathon.Tests.Common;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace Hackathon.Tests.Integration.Base
{
    public class BaseIntegrationTest
    {
        protected readonly IApiService ApiService;

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

            Mapper = factory.Services.GetRequiredService<IMapper>();
            TestFaker = new TestFaker(Mapper);

            var httpClient = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
                HandleCookies = false
            });

            var authToken = userService.GenerateToken(1);
            httpClient.BaseAddress = new Uri("https://localhost:7001/");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken.Token);

            ApiService = new ApiService(httpClient);
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