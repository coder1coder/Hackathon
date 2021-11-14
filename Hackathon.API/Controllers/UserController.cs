using System.Threading.Tasks;
using Hackathon.Common;
using Hackathon.Common.Abstraction;
using Hackathon.Common.Models;
using Hackathon.Common.Models.User;
using Hackathon.Contracts.Requests.User;
using Hackathon.Contracts.Responses;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.API.Controllers
{
    public class UserController: BaseController
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public UserController(
            IMapper mapper,
            IUserService userService
            )
        {
            _mapper = mapper;
            _userService = userService;
        }

        /// <summary>
        /// Создание нового пользователя
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<BaseCreateResponse> SignUp([FromBody] SignUpRequest request)
        {
            var signUpModel = _mapper.Map<SignUpModel>(request);
            var userId = await _userService.CreateAsync(signUpModel);
            return new BaseCreateResponse()
            {
                Id = userId
            };
        }

        /// <summary>
        /// Авторизация пользователя
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost(nameof(SignIn))]
        public async Task<AuthTokenModel> SignIn([FromBody] SignInRequest request)
        {
            var signInModel = _mapper.Map<SignInModel>(request);
            return await _userService.SignInAsync(signInModel);
        }
    }
}