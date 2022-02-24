using Amazon.S3;
using Autofac;
using FluentValidation;
using Hackathon.BL.Common.Validators;
using Hackathon.BL.Event.Validators;
using Hackathon.BL.Project.Validators;
using Hackathon.BL.Team.Validators;
using Hackathon.BL.User.Validators;
using Hackathon.Common.Configuration;
using Hackathon.Common.Models;
using Hackathon.Common.Models.Event;
using Hackathon.Common.Models.Project;
using Hackathon.Common.Models.Team;
using Hackathon.Common.Models.User;

namespace Hackathon.BL;

public class Module: Autofac.Module
{
    private readonly AppSettings _appSettings;
    public Module(AppSettings appSettings)
    {
        _appSettings = appSettings;
    }
    
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<ProjectCreateModelValidator>().As<IValidator<ProjectCreateModel>>().InstancePerLifetimeScope();
        builder.RegisterType<CreateTeamModelValidator>().As<IValidator<CreateTeamModel>>().InstancePerLifetimeScope();
        builder.RegisterType<TeamAddMemberModelValidator>().As<IValidator<TeamMemberModel>>().InstancePerLifetimeScope();
        builder.RegisterType<GetFilterModelValidator<TeamFilterModel>>().As<IValidator<GetListModel<TeamFilterModel>>>().InstancePerLifetimeScope();
        builder.RegisterType<SignUpModelValidator>().As<IValidator<SignUpModel>>().InstancePerLifetimeScope();
        builder.RegisterType<SignInModelValidator>().As<IValidator<SignInModel>>().InstancePerLifetimeScope();
        builder.RegisterType<GetFilterModelValidator<UserFilterModel>>().As<IValidator<GetListModel<UserFilterModel>>>().InstancePerLifetimeScope();
        builder.RegisterType<CreateEventModelValidator>().As<IValidator<CreateEventModel>>().InstancePerLifetimeScope();
        builder.RegisterType<UpdateEventModelValidator>().As<IValidator<UpdateEventModel>>().InstancePerLifetimeScope();
        builder.RegisterType<GetFilterModelValidator<EventFilterModel>>().As<IValidator<GetListModel<EventFilterModel>>>().InstancePerLifetimeScope();
        
        builder.RegisterAssemblyTypes(typeof(Module).Assembly)
            .Where(x=> !x.IsAbstract && x.Name.EndsWith("Service"))
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();
        
        builder.RegisterInstance(new AmazonS3Client(
            _appSettings.S3Options.AccessKey,
            _appSettings.S3Options.SecretKey,
            new AmazonS3Config
            {
                UseHttp = _appSettings.S3Options.UseHttp,
                ServiceURL = _appSettings.S3Options.ServiceUrl,
                ForcePathStyle = _appSettings.S3Options.ForcePathStyle
            })).AsSelf();
    }
}