using Application.Wrappers.Common;
using Application.DTOs.Properties;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Application.Contracts.Persistence.Common.UnitOfWork;
using Application.Exceptions;
using System.Data;

namespace Application.Features.Properties.Commands.UpdatePropertyPrice
{
    public class UpdatePropertyPriceCommandHandler : IRequestHandler<UpdatePropertyPriceCommand, BaseWrapperResponse<PropertyDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdatePropertyPriceCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BaseWrapperResponse<PropertyDto>> Handle(UpdatePropertyPriceCommand request, CancellationToken cancellationToken)
        {
            var propRepo = _unitOfWork.Repository<Property>();
            var property = await propRepo.GetByIdAsync(request.IdProperty, cancellationToken)
                ?? throw new NotFoundException("Property", request.IdProperty);

            // Save trace
            var trace = new PropertyTrace
            {
                IdPropertyTrace = Guid.NewGuid(),
                IdProperty = property.IdProperty,
                DateSale = DateTime.UtcNow,
                Name = "Price Update",
                Value = property.Price,
                Tax = 0m
            };

            property.Price = request.NewPrice;
            property.UpdatedAt = DateTime.UtcNow;

            await propRepo.UpdateAsync(property, cancellationToken);

            var traceRepo = _unitOfWork.Repository<PropertyTrace>();
            await traceRepo.AddAsync(trace, cancellationToken);

            await _unitOfWork.Complete();

            var dto = _mapper.Map<PropertyDto>(property);
            return new Wrappers.WrapperResponse<PropertyDto>(dto,"Actualización correctass");
        }
    }
}
