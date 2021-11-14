﻿using System.Threading.Tasks;
using FluentValidation;
using Hackathon.Common;
using Hackathon.Common.Abstraction;
using Hackathon.Common.Entities;
using Hackathon.Common.Exceptions;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Team;
using MapsterMapper;

namespace Hackathon.BL.Team
{
    public class TeamService : ITeamService
    {
        private readonly IMapper _mapper;
        private readonly IValidator<CreateTeamModel> _createTeamModelValidator;
        private readonly ITeamRepository _teamRepository;

        public TeamService(IMapper mapper, IValidator<CreateTeamModel> createTeamModelValidator,
            ITeamRepository teamRepository)
        {
            _mapper = mapper;
            _createTeamModelValidator = createTeamModelValidator;
            _teamRepository = teamRepository;
        }

        public async Task<long> CreateAsync(CreateTeamModel createTeamModel)
        {
            await _createTeamModelValidator.ValidateAndThrowAsync(createTeamModel);
            var canCreate = await _teamRepository.CanCreateAsync(createTeamModel);
            if (!canCreate)
                throw new ServiceException("Невозможно создать команду");
            var createTeamEntity = _mapper.Map<CreateTeamModel>(createTeamModel);

            return await _teamRepository.CreateAsync(createTeamEntity);
        }
    }
}