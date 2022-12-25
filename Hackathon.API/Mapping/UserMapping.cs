using Hackathon.Common.Models.User;
using Hackathon.Contracts.Responses.User;
using Mapster;

namespace Hackathon.API.Mapping;

public class UserMapping: IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.ForType<UserModel, UserResponse>();
    }
}
