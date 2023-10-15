using FluentValidation;
using Hackathon.Common.Models.ApprovalApplications;

namespace Hackathon.BL.Validation.ApprovalApplications;

public class ApprovalApplicationRejectParametersValidator: AbstractValidator<ApprovalApplicationRejectParameters>
{
    public ApprovalApplicationRejectParametersValidator()
    {
        RuleFor(x => x.Comment)
            .NotEmpty()
            .WithMessage("Комментарий к решению не указан")
            .MinimumLength(10)
            .WithMessage("Комментарий к решению слишком короткий");
    }
}
