using BackendTools.Common.Models;
using Hackathon.Common.Models.Base;
using Hackathon.Common.Models.Team;
using System.Threading.Tasks;

namespace Hackathon.Common.Abstraction.Team;

public interface IPrivateTeamService
{
    /// <summary>
    /// Создать запрос на вступление в команду
    /// </summary>
    /// <param name="parameters">Параметры запроса</param>
    /// <returns></returns>
    Task<Result> CreateJoinRequestAsync(TeamJoinRequestCreateParameters parameters);

    /// <summary>
    /// Отменить запрос на вступление в команду
    /// </summary>
    /// <param name="teamId">Идентификатор команды</param>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <returns></returns>
    Task<Result> CancelJoinRequestAsync(long teamId, long userId);

    /// <summary>
    /// Получить отправленный запрос на вступление в команду
    /// </summary>
    /// <param name="teamId">Идентификатор команды</param>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <returns></returns>
    Task<Result<TeamJoinRequestModel>> GetSentJoinRequestAsync(long teamId, long userId);

    /// <summary>
    /// Получить список запросов на вступление в команду
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    Task<BaseCollection<TeamJoinRequestModel>> GetJoinRequestsAsync(long userId, Models.GetListParameters<TeamJoinRequestFilter> parameters);
}
