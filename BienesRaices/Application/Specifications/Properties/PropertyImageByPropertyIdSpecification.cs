using Ardalis.Specification;
using Domain.Entities;

namespace Application.Specifications.Properties
{
    public class PropertyImageByPropertyIdSpecification : SingleResultSpecification<PropertyImage>
    {
        public PropertyImageByPropertyIdSpecification(Guid propertyId)
        {
            Query.Where(x => x.IdProperty == propertyId);
        }
    }
}
