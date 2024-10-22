using Hackathon.Chats.Abstractions.Models.Teams;

namespace Hackathon.Chats.Abstractions.Repositories;

/// <summary>
/// Репозиторий сообщений чата команды
/// </summary>
public interface ITeamChatRepository: IChatRepository<TeamChatMessage>;
