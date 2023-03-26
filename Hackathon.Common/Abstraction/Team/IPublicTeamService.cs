using BackendTools.Common.Models;
using System.Threading.Tasks;

namespace Hackathon.Common.Abstraction.Team;

/// <summary>
/// Логика команд открытого типа
/// </summary>
public interface IPublicTeamService
{
    /// <summary>
    /// Вступить в команду открытого типа
    /// </summary>
    /// <param name="teamId">Идентификатор команды</param>
    /// <param name="authorizedUserId">Идентификатор авторизованного пользователя</param>
    /// <returns></returns>
    Task<Result> JoinToTeamAsync(long teamId, long authorizedUserId);
}
