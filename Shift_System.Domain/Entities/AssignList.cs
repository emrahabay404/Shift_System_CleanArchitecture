using Shift_System.Domain.Common;

namespace Shift_System.Domain.Entities
{
   public class AssignList : BaseAuditableEntity
   {
      public int? ShiftId { get; set; }
      public int? TeamId { get; set; }
      public virtual ShiftList Shift { get; set; }
      public virtual Team Team { get; set; }
   }
}