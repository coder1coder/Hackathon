using Autofac;
using FluentValidation;
using Hackathon.BL.User.Validators;
using Hackathon.Common.Abstraction;
using Hackathon.Common.Models.User;

namespace Hackathon.BL.User
{
    public class UserModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SignUpModelValidator>().As<IValidator<SignUpModel>>().InstancePerLifetimeScope();
            builder.RegisterType<SignInModelValidator>().As<IValidator<SignInModel>>().InstancePerLifetimeScope();
            builder.RegisterType<UserExistValidator>().AsSelf().InstancePerLifetimeScope();

            builder.RegisterType<UserService>().As<IUserService>().InstancePerLifetimeScope();
        }
    }
}