using Shift_System.Domain.Common;

namespace Shift_System.Domain.Entities.Tables
{
    public class TeamEmployee : BaseAuditableEntity
    {
        public Guid? EmployeeId { get; set; }
        public Guid? TeamId { get; set; }
        public bool? IsLeader { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual Team Team { get; set; }
    }
}
