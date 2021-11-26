using Hackathon.Common.Entities;
using Hackathon.Common.Models.User;
using Mapster;

namespace Hackathon.DAL.Mappings
{
    public class UserEntityMapping: IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config
                .ForType<SignUpModel, UserEntity>()
                .PreserveReference(true)
                .Map(x => x.PasswordHash, s => s.Password)
                ;

        }
    }
}