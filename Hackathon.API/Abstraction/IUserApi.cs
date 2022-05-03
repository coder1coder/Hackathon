﻿using System.Threading.Tasks;
using Hackathon.Contracts.Requests.User;
using Hackathon.Contracts.Responses;
using Hackathon.Contracts.Responses.User;
using Refit;

namespace Hackathon.API.Abstraction
{
    public interface IUserApi
    {
        [Post("/api/User")]
        Task<BaseCreateResponse> SignUp([Body] SignUpRequest request);

        [Get("/api/User/{userId})")]
        public Task<UserResponse> Get(long userId);
    }
}