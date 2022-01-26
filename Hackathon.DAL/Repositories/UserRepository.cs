﻿using System.Linq;
using System.Threading.Tasks;
using Hackathon.Common.Abstraction;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.User;
using Hackathon.DAL.Entities;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.DAL.Repositories
{
    public class UserRepository: IUserRepository
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _dbContext;

        public UserRepository(
            IMapper mapper,
            ApplicationDbContext dbContext
            )
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        /// <inheritdoc cref="IUserRepository.CreateAsync(SignUpModel)"/>
        public async Task<long> CreateAsync(SignUpModel signUpModel)
        {
            var entity = _mapper.Map<UserEntity>(signUpModel);

            await _dbContext.Users.AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return entity.Id;
        }

        /// <inheritdoc cref="IUserRepository.GetAsync(long)"/>
        public async Task<UserModel> GetAsync(long userId)
        {
            var entity = await _dbContext.Users
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == userId);

            return _mapper.Map<UserModel>(entity);
        }

        /// <inheritdoc cref="IUserRepository.GetAsync(GetFilterModel{UserFilterModel})"/>
        public async Task<BaseCollectionModel<UserModel>> GetAsync(GetFilterModel<UserFilterModel> getFilterModel)
        {
            var query = _dbContext.Users
                .AsNoTracking()
                .AsQueryable();

            if (getFilterModel.Filter != null)
            {
                if (!string.IsNullOrWhiteSpace(getFilterModel.Filter.Username))
                    query = query.Where(x => x.UserName == getFilterModel.Filter.Username);

                if (!string.IsNullOrWhiteSpace(getFilterModel.Filter.Email))
                    query = query.Where(x => x.Email == getFilterModel.Filter.Email);
            }

            var totalCount = await query.LongCountAsync();

            if (!string.IsNullOrWhiteSpace(getFilterModel.SortBy))
            {
                query = getFilterModel.SortBy switch
                {
                    nameof(UserEntity.UserName) => getFilterModel.SortOrder == SortOrder.Asc
                        ? query.OrderBy(x => x.UserName)
                        : query.OrderByDescending(x => x.UserName),

                    nameof(UserEntity.Email) => getFilterModel.SortOrder == SortOrder.Asc
                        ? query.OrderBy(x => x.Email)
                        : query.OrderByDescending(x => x.Email),

                    nameof(UserEntity.FullName) => getFilterModel.SortOrder == SortOrder.Asc
                        ? query.OrderBy(x => x.FullName)
                        : query.OrderByDescending(x => x.FullName),

                    _ => getFilterModel.SortOrder == SortOrder.Asc
                        ? query.OrderBy(x => x.Id)
                        : query.OrderByDescending(x => x.Id)
                };
            }

            var page = getFilterModel.Page ?? 1;
            var pageSize = getFilterModel.PageSize ?? 1000;

            var userModels = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ProjectToType<UserModel>()
                .ToListAsync();

            return new BaseCollectionModel<UserModel>
            {
                Items = userModels,
                TotalCount = totalCount
            };
        }

        /// <inheritdoc cref="IUserRepository.ExistAsync(long)"/>
        public async Task<bool> ExistAsync(long userId)
        {
            return await _dbContext.Users
                .AsNoTracking()
                .AnyAsync(x => x.Id == userId);
        }
    }
}