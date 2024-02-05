using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Hackathon.BL.Auth;
using Hackathon.Chats.Abstractions.Repositories;
using Hackathon.Client;
using Hackathon.Client.Chat;
using Hackathon.Common.Abstraction.Friend;
using Hackathon.Common.Abstraction.Team;
using Hackathon.Common.Abstraction.User;
using Hackathon.Common.Models.Auth;
using Hackathon.Common.Models.User;
using Hackathon.Configuration;
using Hackathon.Configuration.Auth;
using Hackathon.Contracts.Requests.User;
using Hackathon.FileStorage.BL.Validators;
using Hackathon.Logbook.Abstraction.Repositories;
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

    protected readonly IEventLogRepository EventLogRepository;
    protected readonly IUserRepository UserRepository;
    protected readonly ITeamRepository TeamRepository;
    protected readonly IFriendshipRepository FriendshipRepository;
    protected readonly IMapper Mapper;
    protected readonly ITeamChatRepository TeamChatRepository;
    protected readonly IEventChatRepository EventChatRepository;

    protected readonly TestFaker TestFaker;

    protected readonly DataSettings DataSettings;
    protected readonly EmailSettings EmailSettings;
    private readonly AuthenticateSettings _authenticateSettings;

    private readonly HttpClient _httpClient;

    protected (long Id, string Token) TestUser { get; }

    protected BaseIntegrationTest(TestWebApplicationFactory factory)
    {
        DataSettings = factory.Services.GetRequiredService<IOptions<DataSettings>>().Value;
        EmailSettings = factory.Services.GetRequiredService<IOptions<EmailSettings>>().Value;
        _authenticateSettings = factory.Services.GetRequiredService<IOptions<AuthenticateSettings>>().Value;

        EventLogRepository = factory.Services.GetRequiredService<IEventLogRepository>();
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

        var defaultToken = AuthTokenGenerator.GenerateToken(new GenerateTokenPayload
        {
            UserId = 1,
            UserRole = UserRole.Default
        }, _authenticateSettings.Internal).Token ?? "";

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

        var authTokenModel = AuthTokenGenerator.GenerateToken(new GenerateTokenPayload
        {
            UserId = response.Id,
            UserRole = userRole
        }, _authenticateSettings.Internal);

        return (response.Id, authTokenModel.Token);
    }

    protected async Task<Guid> GetEventImageId()
    {
        await using var memoryStream = new MemoryStream();
        var file = TestFaker.GetEmptyImage(memoryStream, FileImageValidator.MinWidthEventImage, FileImageValidator.MinHeightEventImage);
        var streamPart = new StreamPart(file.OpenReadStream(), file.FileName, file.ContentType, file.Name);
        return await EventsApi.UploadEventImage(streamPart);
    }
}
