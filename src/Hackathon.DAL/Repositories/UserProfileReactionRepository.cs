using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hackathon.Common.Abstraction.User;
using Hackathon.Common.Models.Users;
using Hackathon.DAL.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace Hackathon.DAL.Repositories;

public class UserProfileReactionRepository: IUserProfileReactionRepository
{
    private readonly ApplicationDbContext _dbContext;

    public UserProfileReactionRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<UserProfileReaction> GetReactionsAsync(long userId, long targetUserId)
    {
        var userReactions = await _dbContext.UserReactions
            .AsNoTracking()
            .Select(x=>new
            {
                x.UserId,
                x.TargetUserId,
                x.Reaction
            })
            .FirstOrDefaultAsync(x=>
                x.UserId == userId
                && x.TargetUserId == targetUserId);

        return userReactions?.Reaction ?? default(UserProfileReaction);
    }

    public Task<List<UserProfileReaction>> GetReactionsAsync(long targetUserId) => 
        _dbContext.UserReactions
            .AsNoTracking()
            .Where(x => x.TargetUserId == targetUserId && x.Reaction != UserProfileReaction.None)
            .Select(x => x.Reaction)
            .ToListAsync();


    public async Task UpsertReactionsAsync(long userId, long targetUserId, UserProfileReaction reactions)
    {
        var existsEntity = await _dbContext.UserReactions
            .FirstOrDefaultAsync(x =>
                x.UserId == userId
                && x.TargetUserId == targetUserId);

        if (existsEntity is not null)
        {
            existsEntity.Reaction = reactions;
            _dbContext.Update(existsEntity);
            await _dbContext.SaveChangesAsync();
        }
        else
        {
            _dbContext.Add(new UserReactionEntity
            {
                UserId = userId,
                TargetUserId = targetUserId,
                Reaction = reactions
            });
            await _dbContext.SaveChangesAsync();
        }
    }
}
