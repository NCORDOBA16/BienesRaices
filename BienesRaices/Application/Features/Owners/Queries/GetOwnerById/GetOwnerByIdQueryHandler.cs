using Application.Wrappers.Common;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Application.Contracts.Persistence.Common.UnitOfWork;
using Application.DTOs.Owners;
using Application.Exceptions;

namespace Application.Features.Owners.Queries.GetOwnerById
{
    public class GetOwnerByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetOwnerByIdQuery, BaseWrapperResponse<OwnerDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<BaseWrapperResponse<OwnerDto>> Handle(GetOwnerByIdQuery request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.Repository<Owner>();
            var entity = await repo.GetByIdAsync(request.Id, cancellationToken);
            if (entity == null)
                throw new NotFoundException("Owner", request.Id);

            var dto = _mapper.Map<OwnerDto>(entity);
            return new Wrappers.WrapperResponse<OwnerDto>(dto);
        }
    }
}
