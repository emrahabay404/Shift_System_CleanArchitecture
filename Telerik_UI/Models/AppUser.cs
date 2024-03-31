using Microsoft.AspNetCore.Identity;

namespace Telerik_UI.Models
{
   public class AppUser : IdentityUser<int>
   {

      public string FullName { get; set; }
      public bool Status { get; set; }


   }
}
