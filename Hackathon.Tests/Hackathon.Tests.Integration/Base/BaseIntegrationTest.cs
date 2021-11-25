using System;
using System.Net.Http.Headers;
using Hackathon.API.Client;
using Hackathon.Common.Abstraction;
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

        protected BaseIntegrationTest(TestWebApplicationFactory factory)
        {
            UserRepository = factory.Services.GetRequiredService<IUserRepository>();
            EventRepository = factory.Services.GetRequiredService<IEventRepository>();
            TeamRepository = factory.Services.GetRequiredService<ITeamRepository>();
            ProjectRepository = factory.Services.GetRequiredService<IProjectRepository>();

            var userService = factory.Services.GetRequiredService<IUserService>();

            Mapper = factory.Services.GetRequiredService<IMapper>();

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
    }
}