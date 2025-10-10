using Application.Contracts.Persistence.Common.UnitOfWork;
using Application.DTOs.Properties;
using Application.Specifications.Properties;
using Application.Wrappers.Common;
using AutoMapper;
using MediatR;

namespace Application.Features.Properties.Queries.ListProperties
{
    public class GetPropertiesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetPropertiesQuery, BaseWrapperResponse<IList<PropertyDto>>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<BaseWrapperResponse<IList<PropertyDto>>> Handle(GetPropertiesQuery request, CancellationToken cancellationToken)
        {
            var spec = new PropertiesFilterSpecification(
                request.IdOwner,
                request.OwnerName,
                request.PriceFrom,
                request.PriceTo,
                request.Year,
                request.Search,
                request.HasActiveImages,
                request.LastSaleMinValue,
                request.LastSaleTax,
                request.LastSaleFrom,
                request.LastSaleTo);

            var repo = _unitOfWork.Repository<Domain.Entities.Property>();
            var list = await repo.ListAsync(spec, cancellationToken);

            
            var dtos = _mapper.Map<IList<PropertyDto>>(list);
            return new Application.Wrappers.WrapperResponse<IList<PropertyDto>>(dtos);
        }
    }
}
