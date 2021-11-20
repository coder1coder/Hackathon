using Autofac;
using FluentValidation;
using Hackathon.BL.Event.Validators;
using Hackathon.Common.Abstraction;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Event;

namespace Hackathon.BL.Event
{
    public class EventModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CreateEventModelValidator>().As<IValidator<CreateEventModel>>();
            builder.RegisterType<GetFilterModelValidator>().As<IValidator<GetFilterModel<EventFilterModel>>>();
            builder.RegisterType<EventExistValidator>().AsSelf();

            builder.RegisterType<EventService>().As<IEventService>().InstancePerLifetimeScope();
        }
    }
}