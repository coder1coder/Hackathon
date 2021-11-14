using System.Net.Http;
using System.Threading.Tasks;
using Hackathon.API.Client.Base;
using Hackathon.Common.Models.Event;

namespace Hackathon.API.Client.Event
{
    public class EventApiClient: BaseApiClient, IEventClient
    {
        private const string Endpoint = "api/Event";

        public EventApiClient(HttpClient httpClient) : base(httpClient)
        {
        }

        public Task<EventModel> Get(long id)
        {
            return GetAsync<EventModel>($"{Endpoint}/{id}");
        }
    }
}