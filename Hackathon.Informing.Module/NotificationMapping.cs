using System;
using System.Text.Json;
using Hackathon.Informing.Abstractions.Models.Notifications;
using Hackathon.Informing.Abstractions.Models.Notifications.Data;
using Hackathon.Informing.DAL.Entities;
using Mapster;

namespace Hackathon.Informing.Module;

public class NotificationMapping: IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.ForType<CreateNotificationModel<SystemNotificationData>, NotificationEntity>()
            .Map(x => x.Id, _ => Guid.NewGuid())
            .Map(x => x.CreatedAt, _ => DateTime.UtcNow)
            .Map(x => x.Type, s=>s.Type)
            .AfterMapping((s, x) =>
            {
                x.Data = JsonSerializer.Serialize(s.Data);
            });
        
        config.ForType<CreateNotificationModel<TeamJoinRequestDecisionData>, NotificationEntity>()
            .Map(x => x.Id, _ => Guid.NewGuid())
            .Map(x => x.CreatedAt, _ => DateTime.UtcNow)
            .Map(x => x.Type, s=>s.Type)
            .AfterMapping((s, x) =>
            {
                x.Data = JsonSerializer.Serialize(s.Data);
            });
    }
}
