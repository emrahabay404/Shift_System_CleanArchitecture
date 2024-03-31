using Shift_System.Application.Common.Mappings;
using Shift_System.Domain.Entities;

namespace Shift_System.Application.Features.Shift
{
   public class GetAllShiftsDto : IMapFrom<ShiftList>
   {
      public int Id { get; init; }
      public string Shift_Name { get; set; }
   }
}
