using Hackathon.Common.Models.Event;
using Hackathon.Contracts.Requests.Event;
using Hackathon.Contracts.Responses;
using Refit;

namespace Hackathon.Client;

public interface IEventApi
{
    private const string BaseRoute = "/api/Event";

    [Post(BaseRoute)]
    Task<BaseCreateResponse> Create([Body] CreateEventRequest createEventRequest);

    [Put(BaseRoute + "/SetStatus")]
    Task SetStatus([Body] SetStatusRequest<EventStatus> setStatusRequest);

    [Get(BaseRoute + "/{id}")]
    Task<IApiResponse<EventModel>> Get(long id);

    [Post(BaseRoute + "/{id}/join")]
    Task Join(long id);

    /// <summary>
    /// Переключить событие на следующий этап
    /// </summary>
    /// <param name="eventId">Идентификатор события</param>
    /// <returns></returns>
    [Post(BaseRoute + "/{eventId}/stages/next")]
    Task GoNextStage(long eventId);
}
