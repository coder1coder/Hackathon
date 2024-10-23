using Hackathon.Chats.Abstractions.Models.Teams;

namespace Hackathon.Chats.Abstractions.Services;

public interface ITeamChatService: IChatService<NewTeamChatMessage, TeamChatMessage>;
