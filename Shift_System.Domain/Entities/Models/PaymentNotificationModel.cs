namespace Shift_System.Domain.Entities.Models
{
    public class PaymentNotificationModel
    {
        public string merchant_oid { get; set; }
        public string status { get; set; }
        public string total_amount { get; set; }
        public string hash { get; set; }
    }
}
