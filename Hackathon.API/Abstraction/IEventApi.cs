using System.Threading.Tasks;
using Hackathon.Common.Models.Event;
using Hackathon.Contracts.Requests.Event;
using Hackathon.Contracts.Responses;
using Refit;

namespace Hackathon.API.Abstraction
{
    public interface IEventApi
    {
        [Post("/v1/Event")]
        Task<BaseCreateResponse> Create([Body] CreateEventRequest createEventRequest);

        [Put("/v1/Event/SetStatus")]
        Task SetStatus([Body] SetStatusRequest<EventStatus> setStatusRequest);

        [Get("/v1/Event/{id}")]
        Task<EventModel> Get(long id);

        [Post("/v1/Event/{id}/join")]
        Task Join(long id);
    }
}