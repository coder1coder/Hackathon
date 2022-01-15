using System.Threading.Tasks;
using Hackathon.API.Abstraction;
using Hackathon.Common.Abstraction;
using Hackathon.Common.Models;
using Hackathon.Common.Models.User;
using Hackathon.Contracts.Requests.User;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.API.Controllers
{
    public class AdminController:BaseController, IAdminApi
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public AdminController(IMapper mapper, IUserService userService)
        {
            _mapper = mapper;
            _userService = userService;
        }

        /// <summary>
        /// Авторизация администратора
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
    }
}