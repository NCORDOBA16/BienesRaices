using System.Net;
using Application.Exceptions;
using Application.Extensions.Exceptions;
using Application.Extensions.Values;
using Application.Wrappers;
using Microsoft.EntityFrameworkCore;


namespace BienesRaicesAPI.Middlewares
{
    public class ErrorHandlerMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                await HandleError(context, error);
            }
        }

        private static async Task HandleError(HttpContext context, Exception error)
        {
            var response = context.Response;
            if (!response.HasStarted)
            {
                response.ContentType = "application/json";
            }

            var responseModel = new WrapperResponse<string>() { Succeeded = false, Message = error.Message };

            switch (error)
            {
                case NullFileException e:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    responseModel.Errors = e.Message.ValueToOneItemList();
                    break;

                case NotFoundException:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;

                case ApiException:
                    //custom application error
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;

                case ValidationException e:
                    //custom application error
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    responseModel.Errors = e.Errors;
                    break;

                case FluentValidation.ValidationException e:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    responseModel.Errors = [.. e.Errors.Select(x => x.ErrorMessage)];
                    break;

                case DbUpdateException e:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    responseModel.Errors = e.GetMessageFromDbUpdateException().ValueToOneItemList();
                    break;

                case KeyNotFoundException:
                    //not found error
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;

                case ExternalApiException:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    responseModel.Errors = [error.Message];
                    break;

                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }
            var result = System.Text.Json.JsonSerializer.Serialize(responseModel);

            await response.WriteAsync(result);
        }
    }
}
