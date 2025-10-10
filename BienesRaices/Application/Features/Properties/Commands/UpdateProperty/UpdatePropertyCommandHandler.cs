using Application.Wrappers.Common;
using Application.DTOs.Properties;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Application.Contracts.Persistence.Common.UnitOfWork;
using Application.Exceptions;

namespace Application.Features.Properties.Commands.UpdateProperty
{
    public class UpdatePropertyCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<UpdatePropertyCommand, BaseWrapperResponse<PropertyDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<BaseWrapperResponse<PropertyDto>> Handle(UpdatePropertyCommand request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.Repository<Property>();
            var property = await repo.GetByIdAsync(request.IdProperty, cancellationToken) ?? throw new NotFoundException("Property", request.IdProperty);

            if (!string.IsNullOrWhiteSpace(request.Title)) property.Name = request.Title;
            if (!string.IsNullOrWhiteSpace(request.Description)) property.Address = request.Description;
            if (request.Price.HasValue) property.Price = request.Price.Value;
            if (!string.IsNullOrWhiteSpace(request.CodeInternal)) property.CodeInternal = request.CodeInternal;
            if (request.Year.HasValue) property.Year = request.Year.Value;

            property.UpdatedAt = DateTime.UtcNow;

            await repo.UpdateAsync(property, cancellationToken);
            await _unitOfWork.Complete();

            var dto = _mapper.Map<PropertyDto>(property);
            return new Application.Wrappers.WrapperResponse<PropertyDto>(dto);
        }
    }
}
