using BienesRaicesAPI.Middlewares;

namespace BienesRaicesAPI.Extensions
{
    public static class AppExtension
    {
        public static void UseErrorHandlingMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<NotFoundMiddleware>();
            app.UseMiddleware<ErrorHandlerMiddleware>();
        }
    }
}
