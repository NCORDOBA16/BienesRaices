using Domain.Entities.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("Property")] 
    public partial class Property : BaseEntity
    {
        public Guid IdProperty { get; set; }
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public decimal Price { get; set; }
        public string CodeInternal { get; set; } = null!;
        public int? Year { get; set; }
        public Guid IdOwner { get; set; }

        // Navigation
        public virtual Owner? Owner { get; set; }
        public virtual ICollection<PropertyImage>? PropertyImages { get; set; }
        public virtual ICollection<PropertyTrace>? PropertyTraces { get; set; }
    }
}
