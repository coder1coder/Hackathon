using System.Threading.Tasks;
using Hackathon.Abstraction.User;
using Hackathon.API.Abstraction;
using Hackathon.Common.Models;
using Hackathon.Common.Models.User;
using Hackathon.Contracts.Requests.User;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.API.Controllers
{
    /// <summary>
    /// Авторизация и аутентификация
    /// </summary>
    public class AuthController: BaseController, IAuthApi
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public AuthController(IMapper mapper, IUserService userService)
        {
            _mapper = mapper;
            _userService = userService;
        }

        /// <summary>
        /// Авторизация пользователя
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost(nameof(SignIn))]
        public async Task<AuthTokenModel> SignIn([FromBody] SignInRequest request)
        {
            var signInModel = _mapper.Map<SignInModel>(request);
            return await _userService.SignInAsync(signInModel);
        }

        /// <summary>
        /// Авторизация пользователя через Google
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost(nameof(SignInByGoogle))]
        public async Task<AuthTokenModel> SignInByGoogle([FromBody] SignInGoogleRequest request)
        {
            var signInByGoogleModel = _mapper.Map<SignInByGoogleModel>(request);
            return await _userService.SignInByGoogle(signInByGoogleModel);
        }
    }
}