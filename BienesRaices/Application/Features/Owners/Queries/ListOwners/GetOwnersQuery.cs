using Application.Wrappers.Common;
using MediatR;
using Application.DTOs.Owners;

namespace Application.Features.Owners.Queries.ListOwners
{
    public class GetOwnersQuery : IRequest<BaseWrapperResponse<IEnumerable<OwnerDto>>>
    {
        // opcional: filtros futuros (name, paging)
    }
}
