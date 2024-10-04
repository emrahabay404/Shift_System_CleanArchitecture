using Shift_System.Domain.Common;

namespace Shift_System.Domain.Entities
{
   public class AssignList : BaseAuditableEntity
   {
      public Guid? ShiftId { get; set; }
      public Guid? TeamId { get; set; }
      public virtual ShiftList Shift { get; set; }
      public virtual Team Team { get; set; }
   }
}