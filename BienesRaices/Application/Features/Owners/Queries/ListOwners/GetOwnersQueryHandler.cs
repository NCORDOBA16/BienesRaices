using Application.Wrappers.Common;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Application.Contracts.Persistence.Common.UnitOfWork;
using Application.DTOs.Owners;

namespace Application.Features.Owners.Queries.ListOwners
{
    public class GetOwnersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetOwnersQuery, BaseWrapperResponse<IEnumerable<OwnerDto>>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<BaseWrapperResponse<IEnumerable<OwnerDto>>> Handle(GetOwnersQuery request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.Repository<Owner>();
            var list = await repo.ListAsync(cancellationToken);
            var dtos = _mapper.Map<IEnumerable<OwnerDto>>(list);
            return new Wrappers.WrapperResponse<IEnumerable<OwnerDto>>(dtos);
        }
    }
}
