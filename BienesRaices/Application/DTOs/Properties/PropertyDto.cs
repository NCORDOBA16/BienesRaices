using System;

namespace Application.DTOs.Properties
{
    public class PropertyDto
    {
        public Guid IdProperty { get; set; }
        // Following the requested response shape
        public string PropertyName { get; set; } = null!;
        public string PropertyAddress { get; set; } = null!;
        public decimal Price { get; set; }
        public string CodeInternal { get; set; } = null!;
        public int? Year { get; set; }
        public OwnerDto? Owner { get; set; }
        public string? MainImage { get; set; }
        public int TotalImages { get; set; }
        public DateTime? LastSaleDate { get; set; }
        public decimal? LastSaleValue { get; set; }
    }
}
