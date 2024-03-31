using System.ComponentModel.DataAnnotations;

namespace Telerik_UI.Models.DTOs
{
   public class RoleViewModel
   {
      [Required(ErrorMessage = "Lütfen role adı girin")]
      public string Name { get; set; }
   }
}

