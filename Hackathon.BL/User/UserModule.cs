using Autofac;
using Hackathon.BL.User.Validators;
using Hackathon.Common;
using Hackathon.Common.Abstraction;

namespace Hackathon.BL.User
{
    public class UserModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SignUpModelValidator>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<SignInModelValidator>().AsSelf().InstancePerLifetimeScope();

            builder.RegisterType<UserService>().As<IUserService>().InstancePerLifetimeScope();
        }
    }
}