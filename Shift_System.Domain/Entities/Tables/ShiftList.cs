using Shift_System.Domain.Common;

namespace Shift_System.Domain.Entities.Tables
{
    public class ShiftList : BaseAuditableEntity
    {
        public ShiftList()
        {
            _Assignments = new HashSet<AssignList>();
        }
        public string Shift_Name { get; set; }
        public virtual ICollection<AssignList> _Assignments { get; set; }
    }
}