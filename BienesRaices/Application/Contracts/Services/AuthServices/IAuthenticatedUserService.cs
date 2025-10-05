namespace Application.Contracts.Services.AuthServices
{
    public interface IAuthenticatedUserService
    {
        string GetUsernameFromClaims();
    }
}
