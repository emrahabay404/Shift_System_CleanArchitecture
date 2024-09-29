using Microsoft.AspNetCore.Identity;
using Shift_System.Domain.Common;

namespace Shift_System.Domain.Entities
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }



        //BaseAuditableEntity
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsDeleted { get; set; }
        public bool Status { get; set; }
    }
}
