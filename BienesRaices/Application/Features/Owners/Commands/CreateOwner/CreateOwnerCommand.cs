using Application.Wrappers.Common;
using MediatR;
using Application.DTOs.Owners;

namespace Application.Features.Owners.Commands.CreateOwner
{
    public class CreateOwnerCommand : IRequest<BaseWrapperResponse<OwnerDto>>
    {
        public CreateOwnerDto Owner { get; set; } = null!;
    }
}
