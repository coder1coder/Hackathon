﻿using System.Linq;
using System.Threading.Tasks;
using Hackathon.Abstraction;
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
        public async Task<UserModel> CreateAsync(SignUpModel signUpModel)
        {
            var entity = _mapper.Map<UserEntity>(signUpModel);

            await _dbContext.Set<UserEntity>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<UserModel>(entity);
        }

        /// <inheritdoc cref="IUserRepository.GetAsync(long)"/>
        public async Task<UserModel> GetAsync(long userId)
        {
            var entity = await _dbContext.Users
                .Include(x=>x.GoogleAccount)
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == userId);

            return _mapper.Map<UserModel>(entity);
        }

        /// <inheritdoc cref="IUserRepository.GetByGoogleIdAsync(string)"/>
        public async Task<UserModel> GetByGoogleIdAsync(string googleId)
        {
            var entity = await _dbContext.Users
                .Include(x=>x.GoogleAccount)
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.GoogleAccountId == googleId);

            return entity != null ? _mapper.Map<UserModel>(entity) : null;
        }

        /// <inheritdoc cref="IUserRepository.UpdateGoogleAccount"/>
        public async Task UpdateGoogleAccount(GoogleAccountModel googleAccountModel)
        {
            var entity = await _dbContext.GoogleAccounts
                .FirstOrDefaultAsync(x => x.Id == googleAccountModel.Id);

            if (entity != null)
            {
                _mapper.Map(googleAccountModel, entity);
                await _dbContext.SaveChangesAsync();
            }
        }

        /// <inheritdoc cref="IUserRepository.GetAsync(GetListModel{T})"/>
        public async Task<BaseCollectionModel<UserModel>> GetAsync(GetListModel<UserFilterModel> getListModel)
        {
            var query = _dbContext.Users
                .AsNoTracking()
                .AsQueryable();

            if (getListModel.Filter != null)
            {
                if (!string.IsNullOrWhiteSpace(getListModel.Filter.Username))
                    query = query.Where(x => x.UserName == getListModel.Filter.Username);

                if (!string.IsNullOrWhiteSpace(getListModel.Filter.Email))
                    query = query.Where(x => x.Email == getListModel.Filter.Email);
            }

            var totalCount = await query.LongCountAsync();

            if (!string.IsNullOrWhiteSpace(getListModel.SortBy))
            {
                query = getListModel.SortBy switch
                {
                    nameof(UserEntity.UserName) => getListModel.SortOrder == SortOrder.Asc
                        ? query.OrderBy(x => x.UserName)
                        : query.OrderByDescending(x => x.UserName),

                    nameof(UserEntity.Email) => getListModel.SortOrder == SortOrder.Asc
                        ? query.OrderBy(x => x.Email)
                        : query.OrderByDescending(x => x.Email),

                    nameof(UserEntity.FullName) => getListModel.SortOrder == SortOrder.Asc
                        ? query.OrderBy(x => x.FullName)
                        : query.OrderByDescending(x => x.FullName),

                    _ => getListModel.SortOrder == SortOrder.Asc
                        ? query.OrderBy(x => x.Id)
                        : query.OrderByDescending(x => x.Id)
                };
            }

            var userModels = await query
                .Skip((getListModel.Page - 1) * getListModel.PageSize)
                .Take(getListModel.PageSize)
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