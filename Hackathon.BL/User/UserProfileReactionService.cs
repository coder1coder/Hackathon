using System.Collections.Generic;
using System.Threading.Tasks;
using FluentValidation;
using Hackathon.BL.Validation.User;
using Hackathon.Common.Abstraction.User;
using Hackathon.Common.ErrorMessages;
using Hackathon.Common.Models.User;

namespace Hackathon.BL.User;

public class UserProfileReactionService: IUserProfileReactionService
{
    private readonly IUserProfileReactionRepository _userProfileReactionRepository;
    private readonly IUserRepository _userRepository;

    public UserProfileReactionService(IUserProfileReactionRepository userProfileReactionRepository, IUserRepository userRepository)
    {
        _userProfileReactionRepository = userProfileReactionRepository;
        _userRepository = userRepository;
    }

    public async Task UpsertReactionAsync(long userId, long targetUserId, UserProfileReaction reactions)
    {
        await ValidateOrThrow(userId, targetUserId);

        var existsReactions = await _userProfileReactionRepository.GetReactionsAsync(userId, targetUserId);

        if ((existsReactions & reactions) == reactions)
            throw new ValidationException(UserMessages.ReactionAlreadyExistMessage);

        var newReactions = existsReactions | reactions;

        await _userProfileReactionRepository.UpsertReactionsAsync(userId, targetUserId, newReactions);
    }

    public async Task RemoveReactionAsync(long userId, long targetUserId, UserProfileReaction reaction)
    {
        await ValidateOrThrow(userId, targetUserId);

        var reactions = await _userProfileReactionRepository.GetReactionsAsync(userId, targetUserId);

        if ((reactions & reaction) != reaction)
            throw new ValidationException(UserMessages.ReactionNotExistMessage);

        reactions &= ~reaction;

        await _userProfileReactionRepository.UpsertReactionsAsync(userId, targetUserId, reactions);
    }

    public async Task<UserProfileReaction?> GetReactionsAsync(long userId, long targetUserId)
    {
        await ValidateOrThrow(userId, targetUserId);
        return await _userProfileReactionRepository.GetReactionsAsync(userId, targetUserId);
    }

    public async Task<List<UserProfileReactionModel>> GetReactionsByTypeAsync(long targetUserId)
    {
        if (!await _userRepository.ExistsAsync(targetUserId))
            throw new ValidationException(UserErrorMessages.UserDoesNotExists);

        var reactions = await _userProfileReactionRepository.GetReactionsAsync(targetUserId);

        return BuildListReactions(reactions);
    }

    private async Task ValidateOrThrow(long userId, long targetUserId)
    {
        if (userId == targetUserId)
            throw new ValidationException(UserMessages.ReactionsAreNotAvailableOnYourOwnProfile);

        if (! await _userRepository.ExistsAsync(userId))
            throw new ValidationException(UserErrorMessages.UserDoesNotExists);

        if (! await _userRepository.ExistsAsync(targetUserId))
            throw new ValidationException(UserErrorMessages.UserDoesNotExists);
    }

    private List<UserProfileReactionModel> BuildListReactions(List<UserProfileReaction> reactionsList)
    {
        var reactionLike = new UserProfileReactionModel() { Type = UserProfileReaction.Like };
        var reactionHeart = new UserProfileReactionModel() { Type = UserProfileReaction.Heart };
        var reactionFire = new UserProfileReactionModel() { Type = UserProfileReaction.Fire };

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
