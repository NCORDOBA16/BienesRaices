using Application.Wrappers.Common;

namespace Application.Wrappers
{
    public class WrapperResponse<T> : BaseWrapperResponse<T>
    {
        public WrapperResponse() : base() { }
        public WrapperResponse(T data, string? message = null) : base(data, message)
        {
            Succeeded = true;
            Message = message;
            Data = data;
        }

        public WrapperResponse(string message) : base(message)
        {
            Succeeded = false;
            Message = message;
        }
    }
}
