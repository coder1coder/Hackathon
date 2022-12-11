using System.Threading.Tasks;
using Hackathon.Abstraction.Chat;
using Hackathon.Abstraction.IntegrationEvents;
using Hackathon.Abstraction.User;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Chat;
using Hackathon.Entities;
using Hackathon.IntegrationEvents;
using Hackathon.IntegrationEvents.IntegrationEvent;

namespace Hackathon.BL.Chat;

public class ChatService : IChatService
{
    private readonly IChatRepository _chatRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMessageHub<ChatMessageChangedIntegrationEvent> _chatMessageHub;

    public ChatService(
        IChatRepository chatRepository,
        IMessageHub<ChatMessageChangedIntegrationEvent> chatMessageHub,
        IUserRepository userRepository)
    {
        _chatRepository = chatRepository;
        _chatMessageHub = chatMessageHub;
        _userRepository = userRepository;
    }

    public async Task SendMessage(ICreateChatMessage createChatMessage)
    {
        var entity = new ChatMessageEntity
        {
            Context = createChatMessage.Context,
            Message = createChatMessage.Message,
            Timestamp = createChatMessage.Timestamp,
            Type = createChatMessage.Type,
            OwnerId = createChatMessage.OwnerId,
            UserId = createChatMessage.UserId,
        };

        if (createChatMessage is CreateTeamChatMessage createTeamChatMessage)
        {
            entity.TeamId = createTeamChatMessage.TeamId;
        }

        var owner = await _userRepository.GetAsync(createChatMessage.OwnerId);
        entity.OwnerFullName = owner.FullName;

        // enrich message
        if (createChatMessage.UserId.HasValue)
        {
            var user = await _userRepository.GetAsync(createChatMessage.UserId.Value);
            entity.UserFullName = user.FullName;
        }

        await _chatRepository.AddMessage(entity);
        await _chatMessageHub.Publish(TopicNames.ChatMessageChanged, new ChatMessageChangedIntegrationEvent
        {
            Context = createChatMessage.Context,
            TeamId = createChatMessage is CreateTeamChatMessage teamChatMessage
                ? teamChatMessage.TeamId
                : null,
        });
    }

    public async Task<BaseCollection<TeamChatMessage>> GetTeamMessages(long teamId, int offset = 0, int limit = 300)
        => await _chatRepository.GetTeamChatMessages(teamId, offset, limit);
}
