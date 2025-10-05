using Application.Contracts.Persistence.Common.UnitOfWork;
using Application.Contracts.Services.FileServices;
using Application.Specifications.Properties;
using Application.Wrappers;
using Application.Wrappers.Common;
using Domain.Entities;
using MediatR;

namespace Application.Features.Properties.Commands.UploadPropertyImage
{
    public class UploadPropertyImageCommandHandler(IUnitOfWork unitOfWork, IImageUploadService imageService)
        : IRequestHandler<UploadPropertyImageCommand, BaseWrapperResponse<string>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IImageUploadService _imageService = imageService;

        public async Task<BaseWrapperResponse<string>> Handle(UploadPropertyImageCommand request, CancellationToken cancellationToken)
        {
            _ = await _unitOfWork.Repository<Property>().GetByIdAsync(request.PropertyId, cancellationToken)
                ?? throw new KeyNotFoundException($"Property with Id: {request.PropertyId}");
            var imageUrl = await _imageService.UploadImageAsync(request.PropertyImage);

            var spec = new PropertyImageByPropertyIdSpecification(request.PropertyId);
            var propertyImage = await _unitOfWork.Repository<PropertyImage>().FirstOrDefaultAsync(spec, cancellationToken);

            if (propertyImage == null)
            {
                await AddPropertyImageAsync(request, imageUrl, cancellationToken);
            }
            else
            {
                await UpdatePrpertyImageAsync(imageUrl, propertyImage, cancellationToken);
            }

            await _unitOfWork.Complete();

            return new WrapperResponse<string>(data: imageUrl);
        }

        private async Task UpdatePrpertyImageAsync(string imageUrl, PropertyImage propertyImage, CancellationToken cancellationToken)
        {
            propertyImage.File = imageUrl;
            _ = await _unitOfWork.Repository<PropertyImage>().UpdateAsync(propertyImage, cancellationToken);
        }

        private async Task AddPropertyImageAsync(UploadPropertyImageCommand request, string imageUrl, CancellationToken cancellationToken)
        {
            _ = await _unitOfWork.Repository<PropertyImage>().AddAsync(new PropertyImage
            {
                IdProperty = request.PropertyId,
                File = imageUrl
            }, cancellationToken);
        }
    }
}
