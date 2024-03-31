using Shift_System.Domain.Common;
using Shift_System.Domain.Entities;

namespace Shift_System.Application.Features.Shift
{
   public class ShiftUpdatedEvent : BaseEvent
   {
      public ShiftList _shiftList { get; }

      public ShiftUpdatedEvent(ShiftList shiftList)
      {
         _shiftList = shiftList;
      }
   }
}
