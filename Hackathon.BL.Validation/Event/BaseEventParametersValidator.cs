using FluentValidation;
using Hackathon.Common.Models.Event;

namespace Hackathon.BL.Validation.Event;

public class BaseEventParametersValidator: AbstractValidator<BaseEventParameters>
{
    public BaseEventParametersValidator()
    {
        RuleFor(x => x.Name)
                .NotEmpty()
                .MinimumLength(5)
                .MaximumLength(100);

            RuleFor(x => x.Description)
                .NotEmpty()
                .MinimumLength(100)
                .WithMessage("Описание должно содержать минимум 100 символов");

            RuleFor(x => x.Start)
               .GreaterThan(DateTime.UtcNow.Add(TimeSpan.FromMinutes(1)))
               .WithMessage("Дата начала мероприятия должна быть позже {ComparisonValue}");

            RuleFor(x => x.MaxEventMembers)
                .GreaterThan(1)
                .WithMessage("Максимальное количество участников события должно быть больше {ComparisonValue}");

            RuleFor(x => x.MinTeamMembers)
                .GreaterThan(0)
                .WithMessage("Минимальное количество участников в команде должно быть больше {ComparisonValue}");

            RuleFor(x => x.ChangeEventStatusMessages)
                .Empty()
                .WithMessage("На текущий момент, список сообщений высылаемых командам при смене статусов не поддерживается");

            #region На данный момент, не может быть ситуации, когда свойство ChangeEventStatusMessages не пустое
            //When(x => x.ChangeEventStatusMessages is {Count: > 0}, () =>
            //{
            //    RuleForEach(x => x.ChangeEventStatusMessages).ChildRules(statusMessages =>
            //    {
            //        statusMessages
            //            .RuleFor(x => x.Message)
            //            .NotEmpty()
            //            .WithMessage("Указаны не существующие статусы события");

            //        statusMessages
            //            .RuleFor(x => x.Status)
            //            .IsInEnum()
            //            .WithMessage("Нельзя указывать пустые сообщения для рассылки");
            //    });
            //});
            #endregion

            RuleFor(x => x.Stages).NotEmpty().WithMessage("Необходимо указать хотя бы один этап события");
            RuleFor(x => x.Stages)
                .Must(x => x.GroupBy(e => e.Name)
                    .Select(g => new
                    {
                        g.Key,
                        Count = g.Count()
                    })
                    .Any(z => z.Count > 1) == false)
                .WithMessage("Нельзя указать несколько этапов с одинаковым наименованием")
                .When(x => x.Stages is {Count: > 0});

            When(x => x.Stages is {Count: > 0}, () =>
            {
                RuleForEach(x => x.Stages).ChildRules(stage =>
                {
                    stage
                        .RuleFor(x => x.Duration)
                        .GreaterThan(0);

                    stage
                        .RuleFor(x => x.Name)
                        .NotEmpty()
                        .WithMessage("Необходимо указать наименование для этапа события");
                });
            });

            RuleFor(x => x.Award)
                .NotEmpty()
                .WithMessage("Необходимо указать награду");
    }
}
