using Domain.Entities.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("PropertyImage")]
    public partial class PropertyImage : BaseEntity
    {
        public Guid IdPropertyImage { get; set; }
        public Guid IdProperty { get; set; }

        //[Column("File")]
        public string? File { get; set; }
        public bool Enabled { get; set; }

        // Navigation
        public virtual Property? Property { get; set; }
    }
}
