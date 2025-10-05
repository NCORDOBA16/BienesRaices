using Application.Attributes.Services;
using Application.Contracts.Services.FileServices;
using Application.Statics.FileServer;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Services.FileServices
{
    [RegisterService(ServiceLifetime.Scoped)]
    public class CloudinaryImageUploadService : IImageUploadService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryImageUploadService()
        {

            var account = new Account(
                  CloudinaryCredentials.CloudName
                , CloudinaryCredentials.ApiKey
                , CloudinaryCredentials.ApiSecret);
            _cloudinary = new Cloudinary(account);
        }

        public async Task<string> UploadImageAsync(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
                throw new ArgumentException("No se ha proporcionado una imagen válida.");

            using var stream = imageFile.OpenReadStream();

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(imageFile.FileName, stream),
                Folder = "productos" // Carpeta en Cloudinary
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.StatusCode != System.Net.HttpStatusCode.OK)
                throw new Exception($"Error al subir la imagen: {uploadResult.Error?.Message}");

            return uploadResult.SecureUrl.AbsoluteUri;
        }
    }
}
