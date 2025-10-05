namespace Application.Exceptions
{
    public class UnauthorizedException(string? message = null)
        : Exception(message: message ?? DefaultMessage)
    {
        public const string DefaultMessage = "Not authorized";
    }
}
