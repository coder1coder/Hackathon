using FluentValidation;
using Hackathon.Common.Models;

namespace Hackathon.BL.Common.Validators
{
    public class GetFilterModelValidator<T>: AbstractValidator<GetListParameters<T>> where T: class
    {
        public GetFilterModelValidator()
        {
            RuleFor(x => x.Offset)
                .GreaterThanOrEqualTo(0);

            RuleFor(x=>x.Limit)
                .GreaterThan(0);
        }
    }
}