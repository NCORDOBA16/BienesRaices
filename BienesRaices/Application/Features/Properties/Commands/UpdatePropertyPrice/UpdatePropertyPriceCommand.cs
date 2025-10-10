using Application.DTOs.Properties;
using Application.Wrappers.Common;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Features.Properties.Commands.UpdatePropertyPrice
{
    public class UpdatePropertyPriceCommand : IRequest<BaseWrapperResponse<PropertyDto>>
    {
        public Guid IdProperty { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateSale { get; set; }
        public string Name { get; set; } = null!;
        public decimal NewPrice { get; set; }
        public decimal Value { get; set; }
        public decimal? Tax { get; set; }
    }
}
