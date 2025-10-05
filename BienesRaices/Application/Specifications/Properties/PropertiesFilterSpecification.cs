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

            if (idOwner.HasValue)
                Query.Where(p => p.IdOwner == idOwner.Value);

            if (!string.IsNullOrWhiteSpace(ownerName))
                Query.Where(p => p.Owner != null && p.Owner.Name.Contains(ownerName));

            if (priceFrom.HasValue)
                Query.Where(p => p.Price >= priceFrom.Value);

            if (priceTo.HasValue)
                Query.Where(p => p.Price <= priceTo.Value);

            if (year.HasValue)
                Query.Where(p => p.Year == year.Value);

            if (!string.IsNullOrWhiteSpace(search))
                Query.Where(p => p.Address.Contains(search) || p.Name.Contains(search));

            if (hasActiveImages.HasValue)
            {
                if (hasActiveImages.Value)
                    Query.Where(p => p.PropertyImages.Any(pi => pi.IsActive && pi.Enabled));
                else
                    Query.Where(p => !p.PropertyImages.Any(pi => pi.IsActive && pi.Enabled));
            }

            // Last sale filters: evaluate last PropertyTrace (by DateSale)
            if (lastSaleMinValue.HasValue || lastSaleTax.HasValue || lastSaleFrom.HasValue || lastSaleTo.HasValue)
            {
                Query.Where(p => p.PropertyTraces.Any());

                if (lastSaleMinValue.HasValue)
                    Query.Where(p => p.PropertyTraces.OrderByDescending(t => t.DateSale).First().Value >= lastSaleMinValue.Value);

                if (lastSaleTax.HasValue)
                    Query.Where(p => p.PropertyTraces.OrderByDescending(t => t.DateSale).First().Tax == lastSaleTax.Value);

                if (lastSaleFrom.HasValue)
                    Query.Where(p => p.PropertyTraces.OrderByDescending(t => t.DateSale).First().DateSale >= lastSaleFrom.Value);

                if (lastSaleTo.HasValue)
                    Query.Where(p => p.PropertyTraces.OrderByDescending(t => t.DateSale).First().DateSale <= lastSaleTo.Value);
            }
        }
    }
}
