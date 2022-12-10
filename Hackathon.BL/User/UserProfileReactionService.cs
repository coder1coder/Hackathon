using System.Threading.Tasks;
using FluentValidation;
using Hackathon.Abstraction.User;
using Hackathon.BL.Validation;
using Hackathon.Common.Models.User;

namespace Hackathon.BL.User;

public class UserProfileReactionService: IUserProfileReactionService
{
    public const string ReactionAlreadyExistMessage = "Реакции на профиль пользователя уже существует";
    public const string ReactionNotExistMessage = "Реакции на профиль пользователя не существует";

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
            throw new ValidationException(ReactionAlreadyExistMessage);

        var newReactions = existsReactions | reactions;

        await _userProfileReactionRepository.UpsertReactionsAsync(userId, targetUserId, newReactions);
    }

    public async Task RemoveReactionAsync(long userId, long targetUserId, UserProfileReaction reaction)
    {
        await ValidateOrThrow(userId, targetUserId);

        var reactions = await _userProfileReactionRepository.GetReactionsAsync(userId, targetUserId);

        if ((reactions & reaction) != reaction)
            throw new ValidationException(ReactionNotExistMessage);

        reactions &= ~reaction;

        await _userProfileReactionRepository.UpsertReactionsAsync(userId, targetUserId, reactions);
    }

    public async Task<UserProfileReaction?> GetReactionsAsync(long userId, long targetUserId)
    {
        await ValidateOrThrow(userId, targetUserId);
        return await _userProfileReactionRepository.GetReactionsAsync(userId, targetUserId);
    }

    private async Task ValidateOrThrow(long userId, long targetUserId)
    {
        if (userId == targetUserId)
            throw new ValidationException("Реакции не доступны на собственном профиле");

        if (! await _userRepository.IsExistAsync(userId))
            throw new ValidationException(string.Format(ValidationMessagePatterns.UserNotFound, userId));

        if (! await _userRepository.IsExistAsync(targetUserId))
            throw new ValidationException(string.Format(ValidationMessagePatterns.UserNotFound, targetUserId));
    }
}
