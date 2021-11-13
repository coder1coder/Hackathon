using Autofac;
using FluentValidation;
using Hackathon.BL.Event.Validators;
using Hackathon.Common;
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
            builder.RegisterType<EventService>().As<IEventService>().InstancePerLifetimeScope();
        }
    }
}