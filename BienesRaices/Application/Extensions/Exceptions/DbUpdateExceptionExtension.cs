using Microsoft.EntityFrameworkCore;

namespace Application.Extensions.Exceptions
{
    public static class DbUpdateExceptionExtension
    {
        public const string BaseMessage = "Un error interno ha ocurrido :";

        public static string GetMessageFromDbUpdateException(this DbUpdateException e)
        {
            if (e.InnerException is null)
            {
                return $"{BaseMessage} {e.Message}";
            }

            return $"{BaseMessage} {e.InnerException.Message}";
        }
    }
}
