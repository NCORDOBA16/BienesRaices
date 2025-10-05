using Application.Wrappers.Common;
using MediatR;
using Application.DTOs.Properties;

namespace Application.Features.Properties.Commands.UpdateProperty
{
    public class UpdatePropertyCommand : IRequest<BaseWrapperResponse<PropertyDto>>
    {
        public Guid IdProperty { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public string? CodeInternal { get; set; }
        public int? Year { get; set; }
    }
}
