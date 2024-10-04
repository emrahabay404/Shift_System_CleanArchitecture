using Shift_System.Domain.Common.Interfaces;

namespace Shift_System.Domain.Common
{
    public abstract class BaseAuditableEntity : BaseEntity, IAuditableEntity
    {
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? Status { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
