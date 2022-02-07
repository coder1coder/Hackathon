﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Hackathon.Abstraction;
using Hackathon.Common.Exceptions;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Team;
using Hackathon.DAL.Entities;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.DAL.Repositories
{
    public class TeamRepository : ITeamRepository
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _dbContext;
        public TeamRepository(IMapper mapper, ApplicationDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        /// <inheritdoc cref="ITeamRepository.CreateAsync(CreateTeamModel)"/>
        public async Task<long> CreateAsync(CreateTeamModel createTeamModel)
        {
            var createTeamEntity = _mapper.Map<TeamEntity>(createTeamModel);

            await _dbContext.AddAsync(createTeamEntity);
            await _dbContext.SaveChangesAsync();

            return createTeamEntity.Id;
        }

        /// <inheritdoc cref="ITeamRepository.GetAsync(long)"/>
        public async Task<TeamModel> GetAsync(long teamId)
        {
            var teamEntity = await _dbContext.Teams
                .AsNoTracking()
                .Include(x=>x.TeamEvents)
                .Include(x=>x.Users)
                .FirstOrDefaultAsync(x=>x.Id == teamId);

            if (teamEntity == null)
                throw new Exception("Команда с таким идентификатором не найдена");

            return _mapper.Map<TeamModel>(teamEntity);
        }

        /// <inheritdoc cref="ITeamRepository.GetAsync(GetListModel{T})"/>
        public async Task<BaseCollectionModel<TeamModel>> GetAsync(GetListModel<TeamFilterModel> getListModel)
        {
            var query = _dbContext.Teams
                .Include(x => x.Users)
                .Include(x => x.TeamEvents)
                .AsNoTracking();

            if (getListModel.Filter != null)
            {
                if (getListModel.Filter.Ids != null)
                    query = query.Where(x => getListModel.Filter.Ids.Contains(x.Id));

                if (!string.IsNullOrWhiteSpace(getListModel.Filter.Name))
                    query = query.Where(x => x.Name.ToLower().Contains(getListModel.Filter.Name.ToLower().Trim()));

                if (!string.IsNullOrWhiteSpace(getListModel.Filter.Owner))
                    query = query.Where(x => x.Owner.FullName.ToLower().Contains(getListModel.Filter.Owner.ToLower().Trim()));

                if (getListModel.Filter.QuantityUsersFrom.HasValue)
                    query = query.Where(x =>
                        x.TeamEvents.Any(s => s.Team.Users.Count >= getListModel.Filter.QuantityUsersFrom));

                if (getListModel.Filter.QuantityUsersTo.HasValue)
                    query = query.Where(x =>
                        x.TeamEvents.Any(s => s.Team.Users.Count <= getListModel.Filter.QuantityUsersTo));

                if (getListModel.Filter.EventId.HasValue)
                    query = query.Where(x => x.TeamEvents.Any(s => s.EventId == getListModel.Filter.EventId));

                if (getListModel.Filter.ProjectId.HasValue)
                    query = query.Where(x => x.TeamEvents.Any(s => s.ProjectId == getListModel.Filter.ProjectId));

                if (getListModel.Filter.OwnerId.HasValue)
                    query = query.Where(x => x.OwnerId == getListModel.Filter.OwnerId);
            }

            var totalCount = await query.LongCountAsync();

            if (!string.IsNullOrWhiteSpace(getListModel.SortBy))
            {
                query = getListModel.SortBy switch
                {
                    nameof(TeamEntity.Name) => getListModel.SortOrder == SortOrder.Asc
                        ? query.OrderBy(x => x.Name)
                        : query.OrderByDescending(x => x.Name),

                    _ => getListModel.SortOrder == SortOrder.Asc
                        ? query.OrderBy(x => x.Id)
                        : query.OrderByDescending(x => x.Id)
                };
            }

            var teamModels = await query
                .Skip((getListModel.Page - 1) * getListModel.PageSize)
                .Take(getListModel.PageSize)
                .ProjectToType<TeamModel>(_mapper.Config)
                .ToListAsync();

            return new BaseCollectionModel<TeamModel>
            {
                Items = teamModels,
                TotalCount = totalCount
            };
        }

        /// <inheritdoc cref="ITeamRepository.ExistAsync(string)"/>
        public async Task<bool> ExistAsync(string teamName)
        {
            return await _dbContext.Teams
                .AsNoTracking()
                .AnyAsync(x => x.Name.ToLower() == teamName.ToLower());
        }

        /// <inheritdoc cref="ITeamRepository.ExistAsync(long)"/>
        public async Task<bool> ExistAsync(long teamId)
        {
            return await _dbContext.Teams
                .AsNoTracking()
                .AnyAsync(x => x.Id == teamId);
        }

        /// <inheritdoc cref="ITeamRepository.AddMemberAsync(TeamAddMemberModel)"/>
        public async Task AddMemberAsync(TeamAddMemberModel teamAddMemberModel)
        {
            //clear changeTracker because it contains team
            _dbContext.ChangeTracker.Clear();

            var teamEntity = await _dbContext.Teams
                .SingleOrDefaultAsync(x=>x.Id == teamAddMemberModel.TeamId);

            if (teamEntity == null)
                throw new EntityNotFoundException("Команда с указаным индентификатором не найдена");

            var userEntity = await _dbContext.Users
                .SingleOrDefaultAsync(x => x.Id == teamAddMemberModel.UserId);

            if (userEntity == null)
                throw new EntityNotFoundException("Пользователь с указаным индентификатором не найден");

            teamEntity.Users.Add(userEntity);

            await _dbContext.SaveChangesAsync();
        }
    }
}