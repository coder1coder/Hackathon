using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Hackathon.Abstraction.Event;
using Hackathon.Abstraction.Project;
using Hackathon.Abstraction.Team;
using Hackathon.Abstraction.User;
using Hackathon.API.Abstraction;
using Hackathon.Common.Configuration;
using Hackathon.Common.Models.Team;
using Hackathon.Common.Models.User;
using Hackathon.Contracts.Requests.User;
using Hackathon.DAL;
using Hackathon.DAL.Mappings;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Refit;
using Xunit;

namespace Hackathon.Tests.Integration.Base
{
    public class BaseIntegrationTest: IClassFixture<TestWebApplicationFactory>
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

        protected readonly AppSettings AppSettings;

        private readonly HttpClient _httpClient;
        private readonly IUserService _userService;

        protected (long Id, string Token) TestUser { get; set; }

        protected BaseIntegrationTest(TestWebApplicationFactory factory)
        {
            AppSettings = factory.Services.GetRequiredService<IOptions<AppSettings>>().Value;

            DbContext = factory.Services.GetRequiredService<ApplicationDbContext>();

            UserRepository = factory.Services.GetRequiredService<IUserRepository>();
            EventRepository = factory.Services.GetRequiredService<IEventRepository>();
            TeamRepository = factory.Services.GetRequiredService<ITeamRepository>();
            ProjectRepository = factory.Services.GetRequiredService<IProjectRepository>();

            _userService = factory.Services.GetRequiredService<IUserService>();

            var config = new TypeAdapterConfig();
            config.Apply(new EventEntityMapping(), new TeamEntityMapping(), new UserEntityMapping());
            Mapper = new Mapper(config);
            TestFaker = new TestFaker(Mapper);

            _httpClient = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
                HandleCookies = false
            });
            
            var defaultToken = _userService.GenerateToken(new UserModel
            {
                Id = 1,
                Role = UserRole.Administrator
            }).Token ?? "";

            TestUser = (1, defaultToken);
            
            SetToken(defaultToken);
            
            AuthApi = RestService.For<IAuthApi>(_httpClient);
            UsersApi = RestService.For<IUserApi>(_httpClient);
            EventsApi = RestService.For<IEventApi>(_httpClient);
            TeamsApi = RestService.For<ITeamApi>(_httpClient);
            ProjectsApi = RestService.For<IProjectApi>(_httpClient);
        }

        protected void SetToken(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        protected async Task<(long Id, string Token)> RegisterUser()
        {
            var fakeUser = TestFaker.GetSignUpModels(1).First();
            var fakeRequest = Mapper.Map<SignUpRequest>(fakeUser);
            var response = await UsersApi.SignUp(fakeRequest);

            var authTokenModel = _userService.GenerateToken(new UserModel
            {
                Id = response.Id,
                Role = UserRole.Default
            });

            return (response.Id, authTokenModel.Token);
        }
        protected async Task<(TeamModel Team, long EventId)> CreateTeamWithEvent(long userId)
        {
            var eventModel = TestFaker.GetCreateEventModels(1, userId).First();
            var eventId = await EventRepository.CreateAsync(eventModel);

            var createTeamModel = TestFaker.GetCreateTeamModels(1).First();
            createTeamModel.EventId = eventId;

            var teamModel = Mapper.Map<TeamModel>(createTeamModel);

            teamModel.Id = await TeamRepository.CreateAsync(createTeamModel);
            return (teamModel, eventId);
        }
    }
}