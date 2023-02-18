﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Hackathon.Abstraction.User;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.User;
using Hackathon.Entities.User;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.DAL.Repositories;

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

    public async Task<long> CreateAsync(SignUpModel signUpModel)
    {
        var entity = _mapper.Map<UserEntity>(signUpModel);

        await _dbContext.Users.AddAsync(entity);
        await _dbContext.SaveChangesAsync();

        return entity.Id;
    }

    public async Task<UserModel> GetAsync(long userId)
    {
        var entity = await _dbContext.Users
            .Include(x=>x.EmailConfirmationRequest)
            .Include(x=>x.GoogleAccount)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == userId);

        return entity == null ? null : _mapper.Map<UserModel>(entity);
    }

    public async Task<UserModel> GetByGoogleIdOrEmailAsync(string googleId, string email)
    {
        var entity = await _dbContext.Users
            .Include(x=>x.EmailConfirmationRequest)
            .Include(x=>x.GoogleAccount)
            .AsNoTracking()
            .FirstOrDefaultAsync(x =>
                x.GoogleAccountId == googleId
                ||
                x.EmailConfirmationRequest.IsConfirmed
                && x.EmailConfirmationRequest.Email.ToLower() == email.ToLower()
                && x.Email.ToLower() == email.ToLower());

        return entity is not null ? _mapper.Map<UserModel>(entity) : null;
    }

    public async Task UpdateGoogleAccount(GoogleAccountModel googleAccountModel)
    {
        var entity = await _dbContext.GoogleAccounts
            .FirstOrDefaultAsync(x => x.Id == googleAccountModel.Id);

        if (entity is not null)
        {
            _mapper.Map(googleAccountModel, entity);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task<BaseCollection<UserModel>> GetAsync(GetListParameters<UserFilter> parameters)
    {
        var query = _dbContext.Users
            .Include(x=>x.EmailConfirmationRequest)
            .AsNoTracking()
            .AsQueryable();

        if (parameters.Filter != null)
        {
            if (!string.IsNullOrWhiteSpace(parameters.Filter.Username))
                query = query.Where(x => x.UserName.ToLower() == parameters.Filter.Username.ToLower());

            if (!string.IsNullOrWhiteSpace(parameters.Filter.Email))
                query = query.Where(x =>
                    x.EmailConfirmationRequest.IsConfirmed
                    && x.EmailConfirmationRequest.Email.ToLower() == parameters.Filter.Email.ToLower()
                    && x.Email.ToLower() == parameters.Filter.Email.ToLower());

            if (parameters.Filter.Ids != null)
                query = query.Where(x => parameters.Filter.Ids.Contains(x.Id));

            if (parameters.Filter.ExcludeIds?.Length > 0)
                query = query.Where(x => !parameters.Filter.ExcludeIds.Contains(x.Id));
        }

        var totalCount = await query.LongCountAsync();

        if (!string.IsNullOrWhiteSpace(parameters.SortBy))
        {
            query = parameters.SortBy switch
            {
                nameof(UserEntity.UserName) => parameters.SortOrder == SortOrder.Asc
                    ? query.OrderBy(x => x.UserName)
                    : query.OrderByDescending(x => x.UserName),

                nameof(UserEntity.Email) => parameters.SortOrder == SortOrder.Asc
                    ? query.OrderBy(x => x.Email)
                    : query.OrderByDescending(x => x.Email),

                nameof(UserEntity.FullName) => parameters.SortOrder == SortOrder.Asc
                    ? query.OrderBy(x => x.FullName)
                    : query.OrderByDescending(x => x.FullName),

                _ => parameters.SortOrder == SortOrder.Asc
                    ? query.OrderBy(x => x.Id)
                    : query.OrderByDescending(x => x.Id)
            };
        }

        var entities = await query
            .Skip(parameters.Offset)
            .Take(parameters.Limit)
            .ToArrayAsync();

        return new BaseCollection<UserModel>
        {
            Items = _mapper.Map<UserEntity[], UserModel[]>(entities),
            TotalCount = totalCount
        };
    }

    public async Task<bool> ExistsAsync(long userId)
    {
        return await _dbContext.Users
            .AsNoTracking()
            .AnyAsync(x => x.Id == userId);
    }

    public async Task UpdateProfileImageAsync(long userId, Guid profileImageId)
    {
        var entity = await _dbContext.Users
            .FirstOrDefaultAsync(x=> x.Id == userId);

        if (entity != null)
        {
            entity.ProfileImageId = profileImageId;
            await _dbContext.SaveChangesAsync();
        }
    }
}
