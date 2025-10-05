using Ardalis.Specification;
using Domain.Entities;

namespace Application.Specifications.Properties
{
    public class PropertyByCodeInternalSpecification : Specification<Property>
    {
        public PropertyByCodeInternalSpecification(string code)
        {
            Query.Where(p => p.CodeInternal == code);
        }
        
    }
    public class OwnerByIdSpecification : Specification<Owner>
    {
        public OwnerByIdSpecification(Guid id)
        {
            Query.Where(o => o.IdOwner == id);
        }
    }
}
