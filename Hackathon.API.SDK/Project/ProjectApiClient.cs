using System.Net.Http;
using System.Threading.Tasks;
using Hackathon.API.Client.Base;
using Hackathon.Contracts.Requests.Project;
using Hackathon.Contracts.Responses;

namespace Hackathon.API.Client.Project
{
    public class ProjectApiClient: BaseApiClient, IProjectApiClient
    {
        private const string Endpoint = "api/Project";

        public ProjectApiClient(HttpClient httpClient) : base(httpClient)
        {
        }

        public async Task<BaseCreateResponse> CreateAsync(ProjectCreateRequest projectCreateRequest)
        {
            return await PostAsync<ProjectCreateRequest, BaseCreateResponse>(Endpoint, projectCreateRequest);
        }
    }
}