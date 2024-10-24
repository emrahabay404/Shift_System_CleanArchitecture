using Shift_System.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shift_System.Domain.Entities.Tables
{
    public class PaymentHistory : BaseAuditableEntity
    {
        public Guid DataId { get; set; }

        // Guid yerine string yapıyoruz
        public string UserId { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }
        public string PayMethod { get; set; } = string.Empty;
        public bool IsPaid { get; set; }
        // AppUser ile ilişki tanımlıyoruz
        public AppUser User { get; set; }
    }
}