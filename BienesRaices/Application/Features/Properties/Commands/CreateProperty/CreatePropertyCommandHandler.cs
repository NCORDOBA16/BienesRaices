using Application.Wrappers.Common;
using Ardalis.Specification;
using Application.DTOs.Properties;
using Application.Mappings.Profiles;
using Application.Specifications.Properties;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Application.Contracts.Persistence.Common.UnitOfWork;
using Application.Exceptions;

namespace Application.Features.Properties.Commands.CreateProperty
{
    public class CreatePropertyCommandHandler : IRequestHandler<CreatePropertyCommand, BaseWrapperResponse<PropertyDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreatePropertyCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BaseWrapperResponse<PropertyDto>> Handle(CreatePropertyCommand request, CancellationToken cancellationToken)
        {
            // Check owner exists
            var ownerRepo = _unitOfWork.Repository<Owner>();;
            _ = await ownerRepo.GetByIdAsync(request.Property.IdOwner, cancellationToken)
            ?? throw new NotFoundException("Property", request.Property.IdOwner);

            // Check duplicate CodeInternal
            var propertyRepo = _unitOfWork.Repository<Property>();
            var spec = new PropertyByCodeInternalSpecification(request.Property.CodeInternal);
            var existing = await propertyRepo.FirstOrDefaultAsync(spec,cancellationToken);
            if (existing != null)
                throw new RecordAlreadyExistException("A property with the same CodeInternal already exists.");

            // Map and persist
            var entity = _mapper.Map<Property>(request.Property);
            await propertyRepo.AddAsync(entity, cancellationToken);
            await _unitOfWork.Complete();

            var dto = _mapper.Map<PropertyDto>(entity);
            return new Wrappers.WrapperResponse<PropertyDto>(dto);
        }
    }
}
