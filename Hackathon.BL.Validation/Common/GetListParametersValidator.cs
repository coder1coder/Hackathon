using FluentValidation;
using Hackathon.Common.Models;

namespace Hackathon.BL.Validation.Common
{
    public class GetListParametersValidator<T>: AbstractValidator<GetListParameters<T>> where T: class
    {
        public GetListParametersValidator()
        {
            RuleFor(x => x.Offset)
                .GreaterThanOrEqualTo(0);

            RuleFor(x=>x.Limit)
                .GreaterThan(0);
        }
    }
}
