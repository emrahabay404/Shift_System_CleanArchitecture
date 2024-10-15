using Shift_System.Domain.Common;

namespace Shift_System.Domain.Entities.Tables
{
    public class PaymentHistory : BaseAuditableEntity
    {
        public Guid DataId { get; set; }
        public Guid UserId { get; set; }
        public decimal Price { get; set; }
        public string PayMethod { get; set; }
    }
}
