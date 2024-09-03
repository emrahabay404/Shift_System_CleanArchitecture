namespace Shift_System_UI.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
        public string? Message { get; set; }
        public int StatusCode { get; set; }  // Hata kodu i�in yeni alan
        public string Url { get; set; }  // Hata kodu i�in yeni alan

    }
}
