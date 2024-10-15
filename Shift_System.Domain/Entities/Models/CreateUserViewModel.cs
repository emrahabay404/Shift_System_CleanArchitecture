using Microsoft.AspNetCore.Identity;

namespace Shift_System.Domain.Entities.Models
{
    public class CreateUserViewModel
    {
        public string Username { get; set; }
        public string FullName { get; set; }  // Yeni FullName alanı
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }  // Cep telefonu alanı
        public List<IdentityRole>? Roles { get; set; }
        public string RoleName { get; set; }  // Seçilen rol ismi
    }
}