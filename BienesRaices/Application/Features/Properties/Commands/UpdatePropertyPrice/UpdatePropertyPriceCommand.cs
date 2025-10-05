using Application.Wrappers.Common;
using MediatR;
using Application.DTOs.Properties;

namespace Application.Features.Properties.Commands.UpdatePropertyPrice
{
    public class UpdatePropertyPriceCommand : IRequest<BaseWrapperResponse<PropertyDto>>
    {
        public Guid IdProperty { get; set; }
        public decimal NewPrice { get; set; }
    }
}
