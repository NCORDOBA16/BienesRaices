using Domain.Entities.Common;

namespace Domain.Entities
{
    public partial class VersionControl : BaseEntity
    {
        public string CurrentVersion { get; set; } = null!;
    }
}
