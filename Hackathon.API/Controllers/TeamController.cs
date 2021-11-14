﻿using System.Threading.Tasks;
using Hackathon.Common.Abstraction;
using Hackathon.Common.Models.Team;
using Hackathon.Contracts.Requests.Team;
using Hackathon.Contracts.Responses;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.API.Controllers
{
    public class TeamController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly ITeamService _teamService;

        public TeamController(ITeamService teamService, IMapper mapper)
        {
            _teamService = teamService;
            _mapper = mapper;
        }

        /// <summary>
        /// Создание новой команды
        /// </summary>
        /// <param name="createTeamRequest"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task<BaseCreateResponse> Create(CreateTeamRequest createTeamRequest)
        {
            var createTeamModel = _mapper.Map<CreateTeamModel>(createTeamRequest);
            var teamId = await _teamService.CreateAsync(createTeamModel);
            return new BaseCreateResponse
            {
                Id = teamId,
            };
        }

        /// <summary>
        /// Регистрация пользователя в команде
        /// </summary>
        /// <param name="teamAddMemberRequest"></param>
        [Authorize]
        [HttpPost(nameof(AddMember))]
        public async Task AddMember(TeamAddMemberRequest teamAddMemberRequest)
        {
            await _teamService.AddMemberAsync(teamAddMemberRequest.TeamId, teamAddMemberRequest.UserId);
        }

        /// <summary>
        /// Получить команду по идентификатору
        /// </summary>
        /// <param name="id"></param>
        [Authorize]
        [HttpGet("{id:long}")]
        public async Task<TeamModel> Get([FromRoute] long id)
        {
            return await _teamService.GetAsync(id);
        }
    }
}