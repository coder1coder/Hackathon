using Hackathon.API.Client.User;

namespace Hackathon.API.Client
{
    public interface IApiService
    {
        public IUserApiClient Users { get; }
    }
}