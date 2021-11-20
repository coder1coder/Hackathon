using Autofac;
using FluentValidation;
using Hackathon.BL.Project.Validators;
using Hackathon.Common.Abstraction;
using Hackathon.Common.Models.Project;

namespace Hackathon.BL.Project
{
    public class ProjectModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterType<ProjectCreateModelValidator>()
                .As<IValidator<ProjectCreateModel>>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ProjectService>().As<IProjectService>().InstancePerLifetimeScope();
        }
    }
}