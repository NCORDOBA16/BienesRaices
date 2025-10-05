using Application.Statics.Enums;

namespace Application.Models.ExternalApi.Common
{
    public class ApiRequest
    {
        public ApiRequestType ApiRequestType { get; set; } = ApiRequestType.Get;
        public string Url { get; set; } = null!;
        public object? Data { get; set; }
        public string? AccessToken { get; set; }
    }
}
