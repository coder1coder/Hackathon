using FluentValidation;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;

namespace Hackathon.BL.Common.Validators
{
    public class GetFilterModelValidator<T>: AbstractValidator<GetFilterModel<T>> where T: IFilterModel
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