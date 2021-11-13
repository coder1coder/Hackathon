﻿using System.Threading.Tasks;
using Hackathon.Contracts.Responses;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Hackathon.Common;
using Hackathon.Common.Abstraction;
using Hackathon.Common.Models;
using Hackathon.Contracts.Requests;
using Hackathon.Contracts.Requests.Team;
using Microsoft.AspNetCore.Authorization;

namespace Hackathon.API.Controllers
{
    // [ApiController]
    // [Route("[controller]")]
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
    }
}