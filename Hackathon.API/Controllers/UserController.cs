using System.Threading.Tasks;
using Hackathon.Abstraction;
using Hackathon.API.Abstraction;
using Hackathon.Common.Models;
using Hackathon.Common.Models.User;
using Hackathon.Contracts.Requests;
using Hackathon.Contracts.Requests.User;
using Hackathon.Contracts.Responses;
using Hackathon.Contracts.Responses.User;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.API.Controllers
{
    public class UserController: BaseController, IUserApi
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        
        /// <summary>
        /// Пользователи
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="userService"></param>
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

        /// <summary>
        /// Получить информацию о пользователе
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{userId:long}")]
        public async Task<UserResponse> Get([FromRoute] long userId)
            => _mapper.Map<UserResponse>(await _userService.GetAsync(userId));

        /// <summary>
        /// Получить информацию о пользователях
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Policy = nameof(UserRole.Administrator))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BaseCollectionResponse<UserResponse>))]
        public async Task<IActionResult> GetAsync([FromQuery] GetListRequest<UserFilterModel> listRequest)
        {
            var getFilterModel = _mapper.Map<GetListModel<UserFilterModel>>(listRequest);
            var collectionModel = await _userService.GetAsync(getFilterModel);
            return Ok(_mapper.Map<BaseCollectionResponse<UserResponse>>(collectionModel));
        }

    }
}