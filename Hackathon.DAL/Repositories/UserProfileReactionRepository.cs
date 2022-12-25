using System.Threading.Tasks;
using Hackathon.Abstraction.User;
using Hackathon.Common.Models.User;
using Hackathon.Entities.User;
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
            .FirstOrDefaultAsync(x=>
                x.UserId == userId
                && x.TargetUserId == targetUserId);

        return userReactions?.Reaction ?? default(UserProfileReaction);
    }

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