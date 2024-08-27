using System.ComponentModel.DataAnnotations;

namespace Shift_System_UI.Models
{
   public class Userloginviewmodel
    {
        [Required(ErrorMessage = "Lütfen kullanıcı girin")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Lütfen şifre girin")]
        public string Password { get; set; }
    }
}
