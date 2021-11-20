using FluentValidation;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Event;

namespace Hackathon.BL.Event.Validators
{
    public class GetFilterModelValidator: AbstractValidator<GetFilterModel<EventFilterModel>>
    {
        public GetFilterModelValidator()
        {
            RuleFor(x=>x.Page)
                .GreaterThan(0)
                .WithMessage("Номер страницы должен быть больше 0");

            RuleFor(x=>x.PageSize)
                .GreaterThan(0)
                .WithMessage("Размер страницы должен быть больше 0");
        }
    }
}