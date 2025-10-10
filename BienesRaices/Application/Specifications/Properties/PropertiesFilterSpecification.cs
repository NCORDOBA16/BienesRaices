using Ardalis.Specification;
using Domain.Entities;

namespace Application.Specifications.Properties
{
    public class PropertiesFilterSpecification : Specification<Property>
    {
        public PropertiesFilterSpecification(
            Guid? idOwner,
            string? ownerName,
            decimal? priceFrom,
            decimal? priceTo,
            int? year,
            string? search,
            bool? hasActiveImages,
            decimal? lastSaleMinValue,
            decimal? lastSaleTax,
            DateTime? lastSaleFrom,
            DateTime? lastSaleTo)
        {
            Query
                .Include(p => p.Owner)
                .Include(p => p.PropertyImages)
                .Include(p => p.PropertyTraces);

            Query.Where(p => (!idOwner.HasValue || p.IdOwner == idOwner)
            && (string.IsNullOrWhiteSpace(ownerName) || p.Owner == null || p.Owner.Name == ownerName)
            && (!priceFrom.HasValue || p.Price >= priceFrom)
            && (!priceTo.HasValue || p.Price <= priceTo)
            && (!year.HasValue || p.Year == year)
            && (string.IsNullOrWhiteSpace(search) || p.Address.Contains(search))
            );


            // Last sale filters: evaluate last PropertyTrace (by DateSale)
            if (lastSaleMinValue.HasValue || lastSaleTax.HasValue || lastSaleFrom.HasValue || lastSaleTo.HasValue)
            {
                Query.Where(p => p.PropertyTraces != null && p.PropertyTraces.Count > 0);

                if (lastSaleMinValue.HasValue)
                    Query.Where(p => p.PropertyTraces == null || p.PropertyTraces.Count == 0 || p.PropertyTraces.OrderByDescending(t => t.DateSale).First().Value >= lastSaleMinValue.Value);

                if (lastSaleTax.HasValue)
                    Query.Where(p => p.PropertyTraces == null || p.PropertyTraces.Count == 0 || p.PropertyTraces.OrderByDescending(t => t.DateSale).First().Tax == lastSaleTax.Value);

                if (lastSaleFrom.HasValue)
                    Query.Where(p => p.PropertyTraces == null || p.PropertyTraces.Count == 0 || p.PropertyTraces.OrderByDescending(t => t.DateSale).First().DateSale >= lastSaleFrom.Value);

                if (lastSaleTo.HasValue)
                    Query.Where(p => p.PropertyTraces == null || p.PropertyTraces.Count == 0 || p.PropertyTraces.OrderByDescending(t => t.DateSale).First().DateSale <= lastSaleTo.Value);
            }
        }
    }
}
