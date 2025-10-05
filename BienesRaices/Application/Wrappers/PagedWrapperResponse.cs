using Application.Wrappers.Common;
using Newtonsoft.Json;

namespace Application.Wrappers
{
    public class PagedWrapperResponse<T> : BaseWrapperResponse<T>
    {
        public PagedWrapperResponse() : base() { }
        public PagedWrapperResponse(T data, int pageNumber, int pageSize, int totalRecords)
        {
            Succeeded = true;
            Message = string.Empty;
            Errors = [];
            Data = data;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalRecords = totalRecords;
            TotalPages = pageSize <= 0 || pageNumber <= 0 ? 0 : Convert.ToInt32(Math.Ceiling((decimal)totalRecords / pageSize));
        }

        public PagedWrapperResponse(string message) : base(message)
        {
        }

        public PagedWrapperResponse(T data, string message) : base(data, message)
        {
        }

        [JsonProperty("pageNumber")]
        public int PageNumber { get; set; } = 0;
        [JsonProperty("pageSize")]
        public int PageSize { get; set; } = 0;
        [JsonProperty("totalPages")]
        public int TotalPages { get; set; } = 0;
        [JsonProperty("totalRecords")]
        public int TotalRecords { get; set; } = 0;
    }
}
