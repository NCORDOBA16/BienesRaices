using Application.Wrappers.Common;
using MediatR;
using Application.DTOs.Owners;

namespace Application.Features.Owners.Queries.GetOwnerById
{
    public class GetOwnerByIdQuery : IRequest<BaseWrapperResponse<OwnerDto>>
    {
        public Guid Id { get; set; }
    }
}
