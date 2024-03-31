using System.ComponentModel.DataAnnotations;

namespace Telerik_UI.Models.DTOs
{
   public class UserRegisterDto
   {
      [Display(Name = "Ad soyad")]
      [Required(ErrorMessage = "Lütfen ad soyad")]
      public required string NameSurname { get; set; }

      [Display(Name = "Şifre")]
      [Required(ErrorMessage = "Lütfen şifre")]
      public required string Password { get; set; }

      [Display(Name = "Şifre Onay")]
      [Compare("Password", ErrorMessage = "Şifre tekrar")]
      public string ConfirmPassword { get; set; }

      [Display(Name = "Mail")]
      [Required(ErrorMessage = "Lütfen ad soyad")]
      public required string Mail { get; set; }

      [Display(Name = "Kullanıcı")]
      [Required(ErrorMessage = "Lütfen girin")]
      public required string Username { get; set; }

      //   public IFormFile ImgUrl { get; set; }
   }
}