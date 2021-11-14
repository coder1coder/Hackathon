using System;
using System.Net;
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

        public async Task PutAsync<TRequest>(string endpoint, TRequest request, bool isUseAuthorization = true)
        {
            if (!isUseAuthorization)
                _httpClient.DefaultRequestHeaders.Authorization = null;

            var requestJson = JsonConvert.SerializeObject(request);
            var content = new StringContent(requestJson, Encoding.UTF8, "application/json");

            await _httpClient.PutAsync(endpoint,  content);
        }

        public async Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest request, bool isUseAuthorization = true)
        {
            if (!isUseAuthorization)
                _httpClient.DefaultRequestHeaders.Authorization = null;

            var requestJson = JsonConvert.SerializeObject(request);
            var content = new StringContent(requestJson, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(endpoint,  content);
            return await GetTypedResponseAsync<TResponse>(response);
        }

        public async Task<TResponse> GetAsync<TResponse>(string endpoint, bool isUseAuthorization = true)
        {
            if (!isUseAuthorization)
                _httpClient.DefaultRequestHeaders.Authorization = null;

            var response = await _httpClient.GetAsync(endpoint);
            return await GetTypedResponseAsync<TResponse>(response);
        }

        private async Task<TResponse> GetTypedResponseAsync<TResponse>(HttpResponseMessage response)
        {
            var result = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                if (response is TResponse typedResponse)
                    return typedResponse;

                return  JsonConvert.DeserializeObject<TResponse>(result);
            }

            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new Exception($"Необходимо указать валидный токен");

            throw new Exception($"Код состояния {response.StatusCode} не поддерживается");
        }
    }
}