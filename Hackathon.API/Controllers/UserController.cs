using System.Threading.Tasks;
using Hackathon.API.Abstraction;
using Hackathon.Common.Abstraction;
using Hackathon.Common.Models.User;
using Hackathon.Contracts.Requests.User;
using Hackathon.Contracts.Responses;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.API.Controllers
{
    public class UserController: BaseController, IUserApi
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
        [AllowAnonymous]
        [HttpPost]
        public async Task<BaseCreateResponse> SignUp([FromBody] SignUpRequest request)
        {
            var signUpModel = _mapper.Map<SignUpModel>(request);
            var userId = await _userService.CreateAsync(signUpModel);
            return new BaseCreateResponse
            {
                Id = userId
            };
        }

        [HttpGet]
        [Route("{userId:long}")]
        public async Task<UserModel> Get([FromRoute] long userId)
        {
            return await _userService.GetAsync(userId);
        }
    }
}