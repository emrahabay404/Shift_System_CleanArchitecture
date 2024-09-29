namespace Shift_System_UI.Models
{
    public class UserRolesViewModel
    {
        public string Username { get; set; }  // Kullanıcı adı
        public string FullName { get; set; }  // Kullanıcının tam adı (AppUser'dan)
        public string Email { get; set; }     // Kullanıcının e-posta adresi
        public string PhoneNumber { get; set; } // Kullanıcının telefon numarası
        public List<string> Roles { get; set; }  // Kullanıcının rollerinin listesi
    }
}