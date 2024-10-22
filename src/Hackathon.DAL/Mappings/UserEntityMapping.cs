using System;
using Hackathon.Common.Models.Users;
using Hackathon.DAL.Entities;
using Hackathon.DAL.Entities.Users;
using Mapster;

namespace Hackathon.DAL.Mappings;

public class UserEntityMapping: IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config
            .ForType<CreateNewUserModel, UserEntity>()
            .PreserveReference(true)
            .Map(x => x.PasswordHash, s => s.Password)
            .MaxDepth(2);

        config.ForType<EmailConfirmationRequestEntity, EmailConfirmationRequestModel>();

        config.ForType<UserEntity, UserModel>()
            .BeforeMapping(x => x.Email = new UserEmailModel())
            .IgnoreMember((member,side)=>
                side == MemberSide.Source
                && member.Name == nameof(UserEntity.Email))
            .Map(x=>x.Email.Address, s=>s.Email)
            .Map(x=>x.Email.ConfirmationRequest, s=>s.EmailConfirmationRequest);

        config.ForType<GoogleAccountEntity, GoogleAccountModel>();

        config.ForType<EmailConfirmationRequestParameters, EmailConfirmationRequestEntity>()
            .Map(x => x.CreatedAt, _ => DateTime.UtcNow)
            .Map(x => x.ConfirmationDate, _ => DateTime.UtcNow,
                s => s.IsConfirmed == true);
    }
}
