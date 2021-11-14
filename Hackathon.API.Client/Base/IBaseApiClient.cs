using System.Threading.Tasks;

namespace Hackathon.API.Client.Base
{
    public interface IBaseApiClient
    {
        Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest request);
    }
}