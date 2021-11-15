using System.Net.Http;
using System.Threading.Tasks;
using Hackathon.API.Client.Base;
using Hackathon.Common.Models.Event;
using Hackathon.Contracts.Requests.Event;
using Hackathon.Contracts.Responses;

namespace Hackathon.API.Client.Event
{
    public class EventApiClient: BaseApiClient, IEventApiClient
    {
        private const string Endpoint = "api/Event";

        public EventApiClient(HttpClient httpClient) : base(httpClient)
        {
        }

        public async Task<BaseCreateResponse> Create(CreateEventRequest createEventRequest)
        {
            return await PostAsync<CreateEventRequest, BaseCreateResponse>(Endpoint, createEventRequest);
        }

        public async Task SetStatus(SetStatusRequest<EventStatus> setStatusRequest)
        {
            await PutAsync($"{Endpoint}/SetStatus", setStatusRequest);
        }

        public async Task SetStartRegistration(SetStartRegistrationRequest setStartRegistrationRequest)
        {
            await PutAsync($"{Endpoint}/SetStartRegistration", setStartRegistrationRequest);
        }

        public Task<EventModel> Get(long id)
        {
            return GetAsync<EventModel>($"{Endpoint}/{id}");
        }
    }
}