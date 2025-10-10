using Application.Contracts.Persistence.Common.UnitOfWork;
using Application.Contracts.Services.FileServices;
using Application.DTOs.Owners;
using Application.Wrappers;
using Application.Wrappers.Common;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.Owners.Commands.CreateOwner
{
    public class CreateOwnerCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IImageUploadService imageService) : IRequestHandler<CreateOwnerCommand, BaseWrapperResponse<OwnerDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly IImageUploadService _imageService = imageService;

        public async Task<BaseWrapperResponse<OwnerDto>> Handle(CreateOwnerCommand request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.Repository<Owner>();

            var entity = _mapper.Map<Owner>(request.Owner);

            if (request.Owner.Photo is not null)
            {
                var imageUrl = await _imageService.UploadImageAsync(request.Owner.Photo);
                entity.Photo = imageUrl; 
            }

            await repo.AddAsync(entity, cancellationToken);
            await _unitOfWork.Complete();

            var dto = _mapper.Map<OwnerDto>(entity);
            return new WrapperResponse<OwnerDto>(dto);
        }
    }
}
