using Hackathon.Chats.Abstractions.Models.Events;

namespace Hackathon.Chats.Abstractions.Services;

public interface IEventChatService: IChatService<NewEventChatMessageModel, EventChatMessage>;
