﻿using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Hackathon.Chats.Abstractions.Repositories;
using Hackathon.Client;
using Hackathon.Client.Chat;
using Hackathon.Common;
using Hackathon.Common.Abstraction.EventLog;
using Hackathon.Common.Abstraction.Friend;
using Hackathon.Common.Abstraction.Team;
using Hackathon.Common.Abstraction.User;
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

[Collection(nameof(ApiTestsCollection))]
public abstract class BaseIntegrationTest
{
    protected readonly IAuthApi AuthApi;
    protected readonly IUserApi UsersApi;
    protected readonly IEventApi EventsApi;
    protected readonly ITeamApi TeamApiClient;
    protected readonly IProjectApiClient ProjectsApiClient;
    protected readonly IFriendshipApi FriendshipApi;
    protected readonly ITeamChatApiClient TeamChatApiClient;
    protected readonly IEventChatApiClient EventChatApiClient;
    protected readonly IApprovalApplicationApiClient ApprovalApplicationApiClient;

    protected readonly IEventLogService EventLogService;
    protected readonly IUserRepository UserRepository;
    protected readonly ITeamRepository TeamRepository;
    protected readonly IFriendshipRepository FriendshipRepository;
    protected readonly IMapper Mapper;
    protected readonly ITeamChatRepository TeamChatRepository;
    protected readonly IEventChatRepository EventChatRepository;

    protected readonly TestFaker TestFaker;

    protected readonly DataSettings DataSettings;
    protected readonly EmailSettings EmailSettings;
    private readonly AuthOptions _authOptions;

    private readonly HttpClient _httpClient;

    protected (long Id, string Token) TestUser { get; }

    protected BaseIntegrationTest(TestWebApplicationFactory factory)
    {
        DataSettings = factory.Services.GetRequiredService<IOptions<DataSettings>>().Value;
        EmailSettings = factory.Services.GetRequiredService<IOptions<EmailSettings>>().Value;
        _authOptions = factory.Services.GetRequiredService<IOptions<AuthOptions>>().Value;

        EventLogService = factory.Services.GetRequiredService<IEventLogService>();
        UserRepository = factory.Services.GetRequiredService<IUserRepository>();
        TeamRepository = factory.Services.GetRequiredService<ITeamRepository>();
        FriendshipRepository = factory.Services.GetRequiredService<IFriendshipRepository>();
        TeamChatRepository = factory.Services.GetRequiredService<ITeamChatRepository>();
        EventChatRepository = factory.Services.GetRequiredService<IEventChatRepository>();

        Mapper = factory.Services.GetRequiredService<IMapper>();
        TestFaker = new TestFaker(Mapper);

        _httpClient = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false,
            HandleCookies = false
        });

        var defaultToken = AuthTokenGenerator.GenerateToken(new UserSignInDetails
        {
            UserId = 1,
            UserRole = UserRole.Default
        }, _authOptions).Token ?? "";

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
        TeamApiClient = RestService.For<ITeamApi>(_httpClient, refitSettings);
        ProjectsApiClient = RestService.For<IProjectApiClient>(_httpClient, refitSettings);
        FriendshipApi = RestService.For<IFriendshipApi>(_httpClient, refitSettings);
        TeamChatApiClient = RestService.For<ITeamChatApiClient>(_httpClient, refitSettings);
        EventChatApiClient = RestService.For<IEventChatApiClient>(_httpClient, refitSettings);
        ApprovalApplicationApiClient = RestService.For<IApprovalApplicationApiClient>(_httpClient, refitSettings);
    }

    protected void SetToken(string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    protected async Task<(long Id, string Token)> RegisterUser(UserRole userRole = UserRole.Default)
    {
        var fakeUser = TestFaker.GetSignUpModels(1).First();
        var fakeRequest = Mapper.Map<SignUpRequest>(fakeUser);
        var response = await UsersApi.SignUp(fakeRequest);

        await UserRepository.SetRole(response.Id, userRole);

        var authTokenModel = AuthTokenGenerator.GenerateToken(new UserSignInDetails
        {
            UserId = response.Id,
            UserRole = userRole
        }, _authOptions);

        return (response.Id, authTokenModel.Token);
    }
}
