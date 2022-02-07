using FluentValidation;
using Hackathon.Common.Models;

namespace Hackathon.BL.Common.Validators
{
    public class GetFilterModelValidator<T>: AbstractValidator<GetListModel<T>> where T: class
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