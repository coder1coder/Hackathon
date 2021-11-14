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
        private readonly string _endpoint;

        protected BaseApiClient(string endpoint, HttpClient httpClient)
        {
            _httpClient = httpClient;
            _endpoint = endpoint;
        }

        public async Task<TResponse> CreateAsync<TRequest, TResponse>(TRequest request)
        {
            var requestJson = JsonConvert.SerializeObject(request);
            var content = new StringContent(requestJson, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(_endpoint,  content);
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