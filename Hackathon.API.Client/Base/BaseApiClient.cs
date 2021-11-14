using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Hackathon.API.Client.Base
{
    public abstract class BaseApiClient: IBaseApiClient
    {
        private readonly HttpClient _httpClient;

        protected BaseApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest request)
        {
            var requestJson = JsonConvert.SerializeObject(request);
            var content = new StringContent(requestJson, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(endpoint,  content);
            var result = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                if (response is TResponse typedResponse)
                    return typedResponse;

                return  JsonConvert.DeserializeObject<TResponse>(result);
            }

            throw new Exception($"Код состояния {response.StatusCode} не поддерживается");
        }
    }
}