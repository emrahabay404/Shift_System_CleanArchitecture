using Shift_System.Domain.Common;

namespace Shift_System.Domain.Entities
{
   public class TeamEmployee : BaseAuditableEntity
   {
      public int? EmployeeId { get; set; }
      public int? TeamId { get; set; }
      public bool? IsLeader { get; set; }
      public virtual Employee Employee { get; set; }
      public virtual Team Team { get; set; }
   }
}
