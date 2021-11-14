using System;
using Hackathon.API.Client;
using Hackathon.Common.Abstraction;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace Hackathon.Tests.Integration.Base
{
    public class BaseIntegrationTest
    {
        protected readonly IApiService ApiService;

        protected readonly IUserRepository UserRepository;

        public BaseIntegrationTest(TestWebApplicationFactory factory)
        {
            var httpClient = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
                HandleCookies = false
            });

            httpClient.BaseAddress = new Uri("https://localhost:7001/");

            ApiService = new ApiService(httpClient);

            UserRepository = factory.Services.GetRequiredService<IUserRepository>();
        }
    }
}