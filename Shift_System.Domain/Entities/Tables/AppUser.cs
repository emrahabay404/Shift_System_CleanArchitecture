using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Shift_System.Domain.Entities.Tables
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }

        // BaseAuditableEntity
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsDeleted { get; set; }
        public bool Status { get; set; }

        // Relationship: One user can have many payment histories
        public ICollection<PaymentHistory> Payments { get; set; } = new List<PaymentHistory>();
    }
}
