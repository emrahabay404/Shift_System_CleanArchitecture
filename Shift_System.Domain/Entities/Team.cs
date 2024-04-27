using Shift_System.Domain.Common;

namespace Shift_System.Domain.Entities
{
   public class Team : BaseAuditableEntity
   {
      public Team()
      {
         _Assignments = new HashSet<AssignList>();
         _TeamEmployees = new HashSet<TeamEmployee>();
      }
      public string TeamName { get; set; }
      public virtual ICollection<AssignList> _Assignments { get; set; }
      public virtual ICollection<TeamEmployee> _TeamEmployees { get; set; }
   }
}
