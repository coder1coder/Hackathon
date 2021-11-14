using System.Threading.Tasks;

namespace Hackathon.API.Client.Base
{
    public interface IBaseApiClient
    {
        Task<TResponse> CreateAsync<TRequest, TResponse>(TRequest request);
    }
}