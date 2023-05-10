using BackendTools.Common.Models;
using Hackathon.Common.Abstraction.Chat;
using Hackathon.Common.Models.Chat;
using Hackathon.Common.Models.Chat.Team;

namespace Hackathon.BL.Validation.Chat;

public class NewTeamChatMessageValidator: INewTeamChatMessageValidator
{
    private readonly FluentValidation.IValidator<INewChatMessage> _modelValidator;

    public NewTeamChatMessageValidator(FluentValidation.IValidator<INewChatMessage> modelValidator)
    {
        _modelValidator = modelValidator;
    }

    public async Task<Result> ValidateAsync(NewTeamChatMessage model)
    {
        var validationResult = await _modelValidator.ValidateAsync(model);

        if (!validationResult.IsValid)
            return Result.NotValid(validationResult.Errors.First().ErrorMessage);

        return Result.Success;
    }
}
