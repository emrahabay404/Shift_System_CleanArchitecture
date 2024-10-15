using Shift_System.Application.Common.Mappings;
using Shift_System.Domain.Entities.Tables;

namespace Shift_System.Application.Features.Products.Queries
{
    public class GetAllProductsDto : IMapFrom<Product>
   {
      public int Id { get; init; }
      public string Name { get; set; }
      public string Category { get; set; }
      public int Stock { get; set; }
      public float Price { get; set; }
   }
}
