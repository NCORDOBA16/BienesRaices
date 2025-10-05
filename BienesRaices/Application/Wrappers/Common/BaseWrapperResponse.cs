using Newtonsoft.Json;

namespace Application.Wrappers.Common
{
    public abstract class BaseWrapperResponse<T>
    {
        protected BaseWrapperResponse() { }
        protected BaseWrapperResponse(T data, string? message = null)
        {
            Succeeded = true;
            Message = message;
            Data = data;
        }

        protected BaseWrapperResponse(string message)
        {
            Succeeded = false;
            Message = message;
        }

        [JsonProperty("succeeded")]
        public bool Succeeded { get; set; }
        [JsonProperty("message")]

        public string? Message { get; set; }
        [JsonProperty("errors")]

        public List<string> Errors { get; set; } = [];
        [JsonProperty("data")]

        public T? Data { get; set; }
    }
}
