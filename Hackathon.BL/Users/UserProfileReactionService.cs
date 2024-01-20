using System.Collections.Generic;
using System.Threading.Tasks;
using BackendTools.Common.Models;
using Hackathon.Common.Abstraction.User;
using Hackathon.Common.Models.User;

namespace Hackathon.BL.Users;

public class UserProfileReactionService: IUserProfileReactionService
{
    private readonly IUserProfileReactionRepository _userProfileReactionRepository;
    private readonly IUserRepository _userRepository;

    public UserProfileReactionService(IUserProfileReactionRepository userProfileReactionRepository, IUserRepository userRepository)
    {
        _userProfileReactionRepository = userProfileReactionRepository;
        _userRepository = userRepository;
    }

    public async Task<Result> UpsertReactionAsync(long userId, long targetUserId, UserProfileReaction reactions)
    {
        var baseValidationResult = await BaseValidation(userId, targetUserId);
        if (!baseValidationResult.IsSuccess)
            return baseValidationResult;

        var existsReactions = await _userProfileReactionRepository.GetReactionsAsync(userId, targetUserId);

        if ((existsReactions & reactions) == reactions)
            return Result.NotValid(UserErrorMessages.ReactionAlreadyExistMessage);

        var newReactions = existsReactions | reactions;

        await _userProfileReactionRepository.UpsertReactionsAsync(userId, targetUserId, newReactions);
        
        return Result.Success;
    }

    public async Task<Result> RemoveReactionAsync(long userId, long targetUserId, UserProfileReaction reaction)
    {
        var baseValidationResult = await BaseValidation(userId, targetUserId);
        if (!baseValidationResult.IsSuccess)
            return baseValidationResult;

        var reactions = await _userProfileReactionRepository.GetReactionsAsync(userId, targetUserId);

        if ((reactions & reaction) != reaction)
            return Result.NotValid(UserErrorMessages.ReactionNotExistMessage);

        reactions &= ~reaction;

        await _userProfileReactionRepository.UpsertReactionsAsync(userId, targetUserId, reactions);
        
        return Result.Success;
    }

    public async Task<Result<UserProfileReaction?>> GetReactionsAsync(long userId, long targetUserId)
    {
        var baseValidationResult = await BaseValidation(userId, targetUserId);
        if (!baseValidationResult.IsSuccess)
            return Result<UserProfileReaction?>.FromErrors(baseValidationResult.Errors);
        
        var reactions = await _userProfileReactionRepository.GetReactionsAsync(userId, targetUserId);
        return Result<UserProfileReaction?>.FromValue(reactions);
    }

    public async Task<Result<List<UserProfileReactionModel>>> GetReactionsByTypeAsync(long targetUserId)
    {
        if (!await _userRepository.ExistsAsync(targetUserId))
            return Result<List<UserProfileReactionModel>>.NotFound(UserErrorMessages.UserDoesNotExists);

        var reactions = await _userProfileReactionRepository.GetReactionsAsync(targetUserId);

        var listReactions = BuildListReactions(reactions);

        return Result<List<UserProfileReactionModel>>.FromValue(listReactions); 
    }

    private async Task<Result> BaseValidation(long userId, long targetUserId)
    {
        if (userId == targetUserId)
            return Result.NotValid(UserErrorMessages.ReactionsAreNotAvailableOnYourOwnProfile);

        if (! await _userRepository.ExistsAsync(userId))
            return Result.NotValid(Validation.Users.UserValidationErrorMessages.UserDoesNotExists);

        if (! await _userRepository.ExistsAsync(targetUserId))
            return Result.NotValid(Validation.Users.UserValidationErrorMessages.UserDoesNotExists);
        
        return Result.Success;
    }

    //TODO: Переписать метод на более универсальное построение списка реакций пользователя
    private static List<UserProfileReactionModel> BuildListReactions(List<UserProfileReaction> reactionsList)
    { 
        var reactionLike = new UserProfileReactionModel { Type = UserProfileReaction.Like };
        var reactionHeart = new UserProfileReactionModel { Type = UserProfileReaction.Heart };
        var reactionFire = new UserProfileReactionModel { Type = UserProfileReaction.Fire };

        foreach (var reactions in reactionsList)
        {
            if ((UserProfileReaction.Like & reactions) == UserProfileReaction.Like)
                reactionLike.Count++;

            if ((UserProfileReaction.Heart & reactions) == UserProfileReaction.Heart)
                reactionHeart.Count++;

            if ((UserProfileReaction.Fire & reactions) == UserProfileReaction.Fire)
                reactionFire.Count++;
        }

        return new List<UserProfileReactionModel> { reactionLike, reactionHeart, reactionFire };
    }
}
