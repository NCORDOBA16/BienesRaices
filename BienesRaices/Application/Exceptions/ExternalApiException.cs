using System.Globalization;

namespace Application.Exceptions
{
    public class ExternalApiException : Exception
    {
        public bool IsSuccess { get; set; } = true;
        public object? Result { get; set; }
        public List<string> ErrorMessages { get; set; } = [];

        public ExternalApiException() : base() { }

        public ExternalApiException(string message) : base(message: message) { }

        public ExternalApiException(string message, params object[] args) : base(String.Format(CultureInfo.CurrentCulture, message, args))
        {
        }

        public string GetOneLineMessage()
        {
            var res = ErrorMessages?.Where(x => x != null && !string.IsNullOrEmpty(x));

            if (res is null || !res.Any())
                return string.Empty;

            return string.Join(" --- ", res);
        }
    }
}
