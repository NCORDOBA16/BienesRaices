using Application.DTOs.Properties;
using Application.Wrappers.Common;
using MediatR;

namespace Application.Features.Properties.Queries.ListProperties
{
    public class GetPropertiesQuery : IRequest<BaseWrapperResponse<IList<PropertyDto>>>
    {
        public Guid? IdOwner { get; set; }
        public string? OwnerName { get; set; }
        public decimal? PriceFrom { get; set; }
        public decimal? PriceTo { get; set; }
        public int? Year { get; set; }
        public string? Search { get; set; } // name or address
        public bool? HasActiveImages { get; set; }

        // Filters on last sale (optional)
        public decimal? LastSaleMinValue { get; set; }
        public decimal? LastSaleTax { get; set; }
        public DateTime? LastSaleFrom { get; set; }
        public DateTime? LastSaleTo { get; set; }
    }
}
