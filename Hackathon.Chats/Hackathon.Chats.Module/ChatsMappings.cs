using Hackathon.Chats.Abstractions.Models.Events;
using Hackathon.Chats.Abstractions.Models.Teams;
using Hackathon.Chats.DAL.Entities;
using Mapster;

namespace Hackathon.Chats.Module;

public class ChatMessageMappings: IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.ForType<EventChatMessage, ChatMessageEntity>()
            .TwoWays()
            .Map(x => x.ChatId, s => s.EventId);

        config.ForType<TeamChatMessage, ChatMessageEntity>()
            .TwoWays()
            .Map(x => x.ChatId, s => s.TeamId);
    }
}
