using Shift_System.Domain.Common;

namespace Shift_System.Domain.Entities
{
   public class Product : BaseAuditableEntity
   {

      public string Name { get; set; }
      public string Category { get; set; }
      public int Stock { get; set; }
      public float Price { get; set; }

   }
}
