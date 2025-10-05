using Application.Models.ExternalApi.Common;

namespace Application.Contracts.Services.ExternalApiServices
{
    public interface IBaseApiService : IDisposable
    {
        ApiResponse ResponseModel { get; set; }
        Task<T?> SendAsync<T>(ApiRequest apiRequest);
    }
}
