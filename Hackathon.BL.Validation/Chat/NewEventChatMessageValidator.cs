using BackendTools.Common.Models;
using Hackathon.Common.Abstraction.Chat;
using Hackathon.Common.Models.Chat;
using Hackathon.Common.Models.Chat.Event;

namespace Hackathon.BL.Validation.Chat;

public class NewEventChatMessageValidator: INewEventChatMessageValidator
{
    private readonly FluentValidation.IValidator<INewChatMessage> _modelValidator;

    public NewEventChatMessageValidator(FluentValidation.IValidator<INewChatMessage> modelValidator)
    {
        _modelValidator = modelValidator;
    }

    public async Task<Result> ValidateAsync(NewEventChatMessage message)
    {
        var validationResult = await _modelValidator.ValidateAsync(message);

        if (!validationResult.IsValid)
            return Result.NotValid(validationResult.Errors.First().ErrorMessage);

        return Result.Success;
    }
}
