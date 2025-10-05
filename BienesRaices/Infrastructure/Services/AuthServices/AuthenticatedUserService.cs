using System.Security.Claims;
using Application.Attributes.Services;
using Application.Contracts.Services.AuthServices;
using Application.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Services.AuthServices
{
    [RegisterService(ServiceLifetime.Transient)]
    public class AuthenticatedUserService(IHttpContextAccessor httpContextAccessor) : IAuthenticatedUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public string GetAuthorizationHeader(string? kind = null)
        {
            kind = !string.IsNullOrEmpty(kind) ? kind : "Bearer";
            var authorizationHeader = _httpContextAccessor.HttpContext?.Request.Headers.Authorization.ToString();
            if (string.IsNullOrEmpty(authorizationHeader?.Trim() ?? string.Empty)) throw new ApiException("Not authorized");

            if (!authorizationHeader!.StartsWith(kind, StringComparison.InvariantCultureIgnoreCase)) throw new ApiException("Not authorized");

            authorizationHeader = authorizationHeader!.Replace(kind, string.Empty, StringComparison.InvariantCultureIgnoreCase).Trim();

            return authorizationHeader!;
        }

        public string GetUsernameFromClaims()
        {
            var userName = _httpContextAccessor.HttpContext?.User?.FindFirstValue("userName");
            if (string.IsNullOrEmpty(userName?.Trim() ?? string.Empty)) throw new ApiException("Not authorized");

            return userName!;
        }
    }
}
