using Domain.Entities.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("Owner")] 
    public partial class Owner : BaseEntity
    {
        public Guid IdOwner { get; set; }
        public string Name { get; set; } = null!;
        public string? Address { get; set; }
        public string? Photo { get; set; }
        public DateTime? Birthday { get; set; }

        // Navigation
        public virtual ICollection<Property>? Properties { get; set; }
    }
}
