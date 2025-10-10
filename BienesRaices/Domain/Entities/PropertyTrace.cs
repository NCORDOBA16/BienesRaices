using Domain.Entities.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("PropertyTrace")]
    public partial class PropertyTrace : BaseEntity
    {
        public Guid IdPropertyTrace { get; set; }
        public Guid IdProperty { get; set; }
        public DateTime DateSale { get; set; }
        public string Name { get; set; } = null!;
        public decimal Value { get; set; }
        public decimal? Tax { get; set; }

        // Navigation
        public virtual Property? Property { get; set; }
    }
}