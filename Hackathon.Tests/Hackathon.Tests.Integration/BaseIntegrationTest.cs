using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Hackathon.Abstraction.EventLog;
using Hackathon.Abstraction.Friend;
using Hackathon.Abstraction.Team;
using Hackathon.Abstraction.User;
using Hackathon.API.Abstraction;
using Hackathon.Common.Configuration;
using Hackathon.Common.Models.User;
using Hackathon.Contracts.Requests.User;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Refit;
using Xunit;

namespace Hackathon.Tests.Integration;

public abstract class BaseIntegrationTest: IClassFixture<TestWebApplicationFactory>
{
    protected readonly IAuthApi AuthApi;
    protected readonly IUserApi UsersApi;
    protected readonly IEventApi EventsApi;
    protected readonly ITeamApi TeamsApi;
    protected readonly IProjectApi ProjectsApi;
    protected readonly IFriendshipApi FriendshipApi;

    protected readonly IEventLogService EventLogService;
    protected readonly IUserRepository UserRepository;
    protected readonly ITeamRepository TeamRepository;
    protected readonly IFriendshipRepository FriendshipRepository;
    protected readonly IMapper Mapper;

    protected readonly TestFaker TestFaker;

    protected readonly AppSettings AppSettings;

    private readonly HttpClient _httpClient;
    private readonly IUserService _userService;

    protected (long Id, string Token) TestUser { get; }

    protected BaseIntegrationTest(TestWebApplicationFactory factory)
    {
        AppSettings = factory.Services.GetRequiredService<IOptions<AppSettings>>().Value;

        EventLogService = factory.Services.GetRequiredService<IEventLogService>();
        UserRepository = factory.Services.GetRequiredService<IUserRepository>();
        TeamRepository = factory.Services.GetRequiredService<ITeamRepository>();
        FriendshipRepository = factory.Services.GetRequiredService<IFriendshipRepository>();

        _userService = factory.Services.GetRequiredService<IUserService>();

        Mapper = factory.Services.GetRequiredService<IMapper>();
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

        var jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };

        var refitSettings = new RefitSettings(new SystemTextJsonContentSerializer(jsonSerializerOptions));

        AuthApi = RestService.For<IAuthApi>(_httpClient, refitSettings);
        UsersApi = RestService.For<IUserApi>(_httpClient, refitSettings);
        EventsApi = RestService.For<IEventApi>(_httpClient, refitSettings);
        TeamsApi = RestService.For<ITeamApi>(_httpClient, refitSettings);
        ProjectsApi = RestService.For<IProjectApi>(_httpClient, refitSettings);
        FriendshipApi = RestService.For<IFriendshipApi>(_httpClient, refitSettings);
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
}