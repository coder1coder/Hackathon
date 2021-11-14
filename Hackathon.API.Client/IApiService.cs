using Hackathon.API.Client.Event;
using Hackathon.API.Client.Team;
using Hackathon.API.Client.User;

namespace Hackathon.API.Client
{
    public interface IApiService
    {
        public IUserApiClient Users { get; }
        public IEventApiClient Events { get; }
        public ITeamApiClient Teams { get; }
    }
}