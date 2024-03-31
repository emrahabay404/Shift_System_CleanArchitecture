using System.ComponentModel.DataAnnotations;

namespace Telerik_UI.Models.DTOs
{
    public class UserLoginDto
    {
        [Required(ErrorMessage = "Please enter user")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Please enter password")]
        public string Password { get; set; }
    }
}

