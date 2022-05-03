using System;
using System.Linq;
using FluentValidation;
using Hackathon.Common.Models.Event;

namespace Hackathon.BL.Event.Validators
{
    public class CreateEventModelValidator: AbstractValidator<CreateEventModel>
    {
        public CreateEventModelValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MinimumLength(5)
                .MaximumLength(100);

            RuleFor(x => x.Start)
               .GreaterThan(DateTime.UtcNow)
               .WithMessage("Дата начала мероприятия должна быть позже текущего времени");

            RuleFor(x => x.DevelopmentMinutes)
                .GreaterThan(0)
                .WithMessage("Продолжительность этапа разработки должна быть больше 0 минут");

            RuleFor(x => x.MaxEventMembers)
                .GreaterThan(1)
                .WithMessage("Максимальное количество участников события должно быть больше 1");

            RuleFor(x => x.MemberRegistrationMinutes)
                .GreaterThan(0)
                .WithMessage("Продолжительность этапа регистрации участников должна быть больше 0 минут");

            RuleFor(x => x.MinTeamMembers)
                .GreaterThan(0)
                .WithMessage("Минимальное количество участников в команде должно быть больше 0");

            RuleFor(x => x.TeamPresentationMinutes)
                .GreaterThan(0)
                .WithMessage("Продолжительность этапа презентации должна быть больше 0 минут");

            RuleFor(x => x.OwnerId)
                .GreaterThan(0)
                .WithMessage("Идентификатор пользователя должен быть больше 0");

            When(x => x.ChangeEventStatusMessages != null && x.ChangeEventStatusMessages.Any(), () =>
            {
                RuleForEach(x => x.ChangeEventStatusMessages).ChildRules(statusMessages =>
                {
                    statusMessages
                        .RuleFor(x => x.Message)
                        .NotEmpty()
                        .WithMessage("Указаны не существующие статусы события");

                    statusMessages
                        .RuleFor(x => x.Status)
                        .IsInEnum()
                        .WithMessage("Нельзя указывать пустые сообщения для рассылки");
                });
            });
        }
    }
}