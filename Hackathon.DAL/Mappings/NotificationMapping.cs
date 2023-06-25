using System;
using System.Text.Json;
using Hackathon.Common.Models.Notification;
using Hackathon.DAL.Entities;
using Mapster;

namespace Hackathon.DAL.Mappings;

public class NotificationMapping: IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.ForType<CreateNotificationModel<MessageNotificationData>, NotificationEntity>()
            .Map(x => x.Id, _ => Guid.NewGuid())
            .Map(x => x.CreatedAt, _ => DateTime.UtcNow)
            .Map(x=>x.Type, s=>s.Type)
            .AfterMapping((s, x) =>
            {
                x.Data = JsonSerializer.Serialize(s.Data);
            });

        config.ForType<NotificationEntity, NotificationModel>()
            .Map(x => x.Group, z => z.Type.ToNotificationGroup());
    }
}
