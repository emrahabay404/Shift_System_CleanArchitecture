using Microsoft.AspNetCore.Identity;

namespace Shift_System.Domain.Entities
{
   public class AppUser : IdentityUser
   {
      public string FullName { get; set; }
      public bool Status { get; set; }
   }
}
