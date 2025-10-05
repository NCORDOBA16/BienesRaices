namespace Application.Contracts.Services.ExternalApiServices
{
    public interface IPokemonService : IBaseApiService
    {
        Task<T?> GetPokemonByName<T>(string name, string? accessToken = null);
    }
}
