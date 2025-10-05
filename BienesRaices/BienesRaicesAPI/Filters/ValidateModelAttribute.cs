using Application.Wrappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BienesRaicesAPI.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState
                    .Where(ms => ms.Value?.Errors.Count > 0)
                    .Select(x => x.Key)
                    .Where((prop, indice) =>
                    !prop.Equals("REQUEST", StringComparison.CurrentCultureIgnoreCase)
                    && !prop.Equals("COMMAND", StringComparison.CurrentCultureIgnoreCase)
                    && !prop.Equals("MODEL", StringComparison.CurrentCultureIgnoreCase)
                    && !prop.Equals("QUERY", StringComparison.CurrentCultureIgnoreCase))
                    .Select(x => x.Replace("$.", string.Empty))
                    .ToList();

                string propsError = errors.Count != 0 ? string.Join(", ", errors) : string.Empty;

                var apiResponse = new WrapperResponse<object>
                {
                    Succeeded = false,
                    Message = "Error en la petición",
                    Errors = [$"Algunos valores no coinciden con los tipos de datos del servicio solicitado: [{propsError}]"]
                };

                context.Result = new BadRequestObjectResult(apiResponse);
            }
        }
    }
}
