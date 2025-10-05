using System.Net.Http.Headers;
using System.Text;
using Application.Contracts.Services.ExternalApiServices;
using Application.Exceptions;
using Application.Models.ExternalApi.Common;
using Application.Statics.Configurations;
using Application.Statics.Enums;
using Newtonsoft.Json;

namespace Infrastructure.Services.ExternalApiServices
{
    public class BaseApiService(IHttpClientFactory httpClientFactory) : IBaseApiService
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
        private bool _disposed = false;

        public ApiResponse ResponseModel { get; set; } = new ApiResponse();

        public async Task<T?> SendAsync<T>(ApiRequest apiRequest)
        {
            try
            {
                var client = CreateClient(apiRequest);
                var message = CreateMessage(apiRequest);

                HttpResponseMessage apiResponseMessage = await client.SendAsync(message);

                if (!apiResponseMessage.IsSuccessStatusCode)
                {
                    throw new ExternalApiException(message: $"Error with status code: '{(int)apiResponseMessage.StatusCode}' - {apiResponseMessage.ReasonPhrase ?? "Something went wrong"}");
                }

                var apiContent = await apiResponseMessage.Content.ReadAsStringAsync();

                var apiResponse = JsonConvert.DeserializeObject<T>(apiContent);

                return apiResponse;

            }
            catch (Exception ex)
            {
                T? apiResponse = GetErrorApiResponse<T>(ex);
                return apiResponse;
            }
        }

        private HttpClient CreateClient(ApiRequest apiRequest)
        {
            var client = _httpClientFactory.CreateClient(ExternalApiSettings.ClientName);
            client.DefaultRequestHeaders.Clear();

            if (!string.IsNullOrEmpty(apiRequest.AccessToken))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiRequest.AccessToken);
            }

            return client;
        }

        private static HttpRequestMessage CreateMessage(ApiRequest apiRequest)
        {
            var message = new HttpRequestMessage
            {
                RequestUri = new Uri(apiRequest.Url),
                Method = GetHttpMethod(apiRequest.ApiRequestType),
            };

            message.Headers.Add("Accept", "application/json");

            if (apiRequest.Data is not null)
            {
                message.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data), Encoding.UTF8, "application/json");
            }

            return message;
        }

        private static HttpMethod GetHttpMethod(ApiRequestType apiRequestType)
        {
            return apiRequestType switch
            {
                ApiRequestType.Post => HttpMethod.Post,
                ApiRequestType.Put => HttpMethod.Put,
                ApiRequestType.Delete => HttpMethod.Delete,
                _ => HttpMethod.Get,
            };
        }

        private static T? GetErrorApiResponse<T>(Exception ex)
        {
            var dto = new ApiResponse
            {
                DisplayMessage = "Error",
                ErrorMessages = [Convert.ToString(ex.Message)],
                IsSuccess = false
            };

            var res = JsonConvert.SerializeObject(dto);
            var apiResponse = JsonConvert.DeserializeObject<T>(res);
            return apiResponse;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;
        }
    }
}
