using Application.Wrappers.Common;
using MediatR;
using Application.DTOs.Properties;

namespace Application.Features.Properties.Commands.CreateProperty
{
    public class CreatePropertyCommand : IRequest<BaseWrapperResponse<PropertyDto>>
    {
        public CreatePropertyDto Property { get; set; } = null!;
    }
}
