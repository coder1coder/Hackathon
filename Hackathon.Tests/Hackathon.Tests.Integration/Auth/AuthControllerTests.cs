﻿using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Hackathon.Common.Configuration;
using Hackathon.Contracts.Requests.User;
using Hackathon.Tests.Integration.Base;
using Xunit;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;

namespace Hackathon.Tests.Integration.Auth
{
    public class AuthControllerTests: BaseIntegrationTest, IClassFixture<TestWebApplicationFactory>
    {
        private TestWebApplicationFactory _factory;
        public AuthControllerTests(TestWebApplicationFactory factory) : base(factory) 
        {
            _factory = factory;
        }

        [Fact]
        public async Task SignIn_ShouldReturn_AuthToken()
        {
            var fakeSignUpRequest = Mapper.Map<SignUpRequest>(TestFaker.GetSignUpModels(1).First());
         
            var signUpResponse = await UsersApi.SignUp(fakeSignUpRequest);

            Assert.NotNull(signUpResponse);

            var signInRequest = Mapper.Map<SignInRequest>(fakeSignUpRequest);

            var signInResponse =  await AuthApi.SignIn(signInRequest);

            Assert.NotNull(signInResponse);

            signInResponse.UserId.Should().Be(signUpResponse.Id);
            signInResponse.Expires.Should().BeGreaterThan(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
            signInResponse.Token.Should().NotBeNullOrWhiteSpace();
        } 
        
        [Fact]
        public async Task AdministratorSignIn_ShouldReturn_AuthToken()
        {
            AdministratorDefaults administratorDefaults = _factory.Services.GetRequiredService<IOptions<AdministratorDefaults>>().Value;

            var adminSignUpRequest = Mapper.Map<SignUpRequest>(new SignUpRequest{
                UserName = administratorDefaults.Login,
                Password = administratorDefaults.Password, 
            });
            
            var adminSignInRequest = Mapper.Map<SignInRequest>(adminSignUpRequest);
            var adminSignInResponse =  await AuthApi.SignIn(adminSignInRequest);
            Assert.NotNull(adminSignInResponse);
     
            adminSignInResponse.Expires.Should().BeGreaterThan(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
            adminSignInResponse.Token.Should().NotBeNullOrWhiteSpace();
        }
    }
}