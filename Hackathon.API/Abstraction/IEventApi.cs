using System.Threading.Tasks;
using Hackathon.Common.Models.Event;
using Hackathon.Contracts.Requests.Event;
using Hackathon.Contracts.Responses;
using Microsoft.AspNetCore.Mvc;
using Refit;

namespace Hackathon.API.Abstraction
{
    public interface IEventApi
    {
        [Post("/api/Event")]
        Task<BaseCreateResponse> Create([Body] CreateEventRequest createEventRequest);

        [Put("/api/Event/SetStatus")]
        Task SetStatus([Body] SetStatusRequest<EventStatus> setStatusRequest);

        [Get("/api/Event/{id}")]
        Task<IActionResult> Get(long id);
    }
}