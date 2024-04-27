using Shift_System.Domain.Common;

namespace Shift_System.Domain.Entities
{
   public class Employee : BaseAuditableEntity
   {
      public Employee()
      {
         _TeamEmployees = new HashSet<TeamEmployee>();
      }
      public int EmployeeCode { get; set; }
      public string Name { get; set; }
      public string SurName { get; set; }
      public string UserName { get; set; }
      public string Mail { get; set; }
      public string Phone { get; set; }
      public string Address { get; set; }
      public string Title { get; set; }
      public bool Activity { get; set; }
      public virtual ICollection<TeamEmployee> _TeamEmployees { get; set; }
   }
}
