using System.Net;
using System.Text.Json;
using Application.Wrappers;

namespace BienesRaicesAPI.Middlewares
{
    public class NotFoundMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;
        private const string Message = "El recurso que estás buscando no existe.";

        public async Task InvokeAsync(HttpContext context)
        {
            await _next(context);

            if (context.Response.StatusCode == (int)HttpStatusCode.NotFound && !context.Response.HasStarted)
            {
                context.Response.ContentType = "application/json";

                var responseModel = new WrapperResponse<string>() { Succeeded = false, Message = Message };
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                responseModel.Errors = [Message];

                var result = JsonSerializer.Serialize(responseModel);
                await context.Response.WriteAsync(result);
            }
        }
    }
}
