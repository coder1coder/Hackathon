#nullable enable
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Hackathon.Abstraction.Friend;
using Hackathon.Common.Exceptions;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Friend;
using Hackathon.Common.Models.User;
using Hackathon.DAL.Extensions;
using Hackathon.Entities;
using Hackathon.Entities.User;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.DAL.Repositories;

public class FriendshipRepository: IFriendshipRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public FriendshipRepository(
        ApplicationDbContext dbContext,
        IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<BaseCollection<UserModel>> GetUsersByFriendshipStatus(long userId, FriendshipStatus status)
    {
        var friendships = await _dbContext.Friendships
            .AsNoTracking()
            .Where(x =>
                (x.ProposerId == userId
                || x.UserId == userId)
            && x.Status == status
            ).ToArrayAsync();

        var friendsIds = friendships
            .SelectMany(x=> new []{ x.ProposerId, x.UserId })
            .Where(x=> x != userId)
            .Distinct().ToArray();

        var friendsEntities = await _dbContext.Users
            .AsNoTracking()
            .Where(x => friendsIds.Contains(x.Id))
            .ToArrayAsync();

        var friends = _mapper.Map<UserEntity[], UserModel[]>(friendsEntities);

        return new BaseCollection<UserModel>
        {
            Items = friends,
            TotalCount = friends.Length
        };
    }

    public async Task CreateOfferAsync(long proposerId, long userId)
    {
        var hasOffer = await _dbContext.Friendships
            .AnyAsync(x =>
                x.ProposerId == proposerId
                && x.UserId == userId);

        if (!hasOffer)
        {
            _dbContext.Friendships.Add(new FriendshipEntity
            {
                Id = Guid.NewGuid(),
                ProposerId = proposerId,
                UserId = userId,
                Status = FriendshipStatus.Pending
            });

            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task UpdateStatusAsync(long proposerId, long userId, FriendshipStatus status)
    {
        var entity = await _dbContext
            .Friendships
            .FirstOrDefaultAsync(x =>
                x.ProposerId == proposerId
                && x.UserId == userId);

        if (entity is not null)
        {
            entity.Status = status;
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task UpdateFriendship(long proposerId, long userId, Friendship parameters)
    {
        var entity = await _dbContext
            .Friendships
            .FirstOrDefaultAsync(x =>
                x.ProposerId == proposerId
                && x.UserId == userId);

        if (entity is null)
            return;

        entity.ProposerId = parameters.ProposerId;
        entity.UserId = parameters.UserId;
        entity.Status = parameters.Status;

        await _dbContext.SaveChangesAsync();
    }

    public async Task<Friendship?> GetOfferAsync(long proposerId, long userId, GetOfferOption option = GetOfferOption.Any)
    {
        Expression<Func<FriendshipEntity, bool>> expression = option switch
        {
            GetOfferOption.Incoming => x => x.UserId == proposerId && x.ProposerId == userId,
            GetOfferOption.Outgoing => x => x.ProposerId == proposerId && x.UserId == userId,
            _ => x =>
                (x.ProposerId == proposerId || x.ProposerId == userId)
                && (x.UserId == proposerId || x.UserId == userId)
        };

        var entity = await _dbContext.Friendships
            .AsNoTracking()
            .FirstOrDefaultAsync(expression);

        return entity?.ToDto();
    }

    public async Task<BaseCollection<Friendship>> GetOffersAsync(long userId, GetListParameters<FriendshipGetOffersFilter> parameters)
    {
        var query = _dbContext.Friendships.AsNoTracking();

        if (parameters.Filter != null)
        {
            query = parameters.Filter.Option switch
            {
                GetOfferOption.Incoming => query.Where(x => x.UserId == userId),
                GetOfferOption.Outgoing => query.Where(x => x.ProposerId == userId),
                _ => query.Where(x=>x.ProposerId == userId || x.UserId == userId)
            };

            if (parameters.Filter.Status.HasValue)
                query = query.Where(x => x.Status == parameters.Filter.Status);
        }

        var total = await query.LongCountAsync();

        return new BaseCollection<Friendship>
        {
            Items = await query
                .Skip(parameters.Offset)
                .Take(parameters.Limit)
                .ProjectToType<Friendship>(_mapper.Config)
                .ToArrayAsync(),
            TotalCount = total
        };
    }

    public async Task RemoveOfferAsync(long proposerId, long userId)
    {
        var offerEntity = await _dbContext.Friendships.FirstOrDefaultAsync(x =>
            x.ProposerId == proposerId && x.UserId == userId
            ||
            x.ProposerId == userId && x.UserId == proposerId);

        if (offerEntity is not null)
        {
            _dbContext.Friendships.Remove(offerEntity);
            await _dbContext.SaveChangesAsync();
        }
    }
}
