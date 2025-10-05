using Microsoft.AspNetCore.Http;

namespace Application.Contracts.Services.FileServices
{
    public interface IImageUploadService
    {
        Task<string> UploadImageAsync(IFormFile imageFile);
    }
}
