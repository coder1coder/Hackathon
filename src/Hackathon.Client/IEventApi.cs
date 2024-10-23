using System;
using System.Threading.Tasks;
using Hackathon.API.Contracts;
using Hackathon.API.Contracts.Events;
using Hackathon.Common.Models.Event;
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
    Task JoinAsync(long id);

    /// <summary>
    /// Переключить событие на следующий этап
    /// </summary>
    /// <param name="eventId">Идентификатор события</param>
    [Post(BaseRoute + "/{eventId}/stages/next")]
    Task GoNextStage(long eventId);

    /// <summary>
    /// Загрузить изображение события
    /// </summary>
    /// <param name="stream">Файл изображения</param>
    [Post(BaseRoute + "/image/upload")]
    [Multipart]
    Task<Guid> UploadEventImage([AliasAs("file")] StreamPart stream);

    /// <summary>
    /// Обновить событие
    /// </summary>
    /// <param name="request"></param>
    [Put(BaseRoute)]
    Task<IApiResponse> Update([Body] UpdateEventRequest request);
}
