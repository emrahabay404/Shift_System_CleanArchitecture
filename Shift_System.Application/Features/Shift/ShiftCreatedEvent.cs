using Shift_System.Domain.Common;
using Shift_System.Domain.Entities;

namespace Shift_System.Application.Features.Shift
{
   public class ShiftCreatedEvent : BaseEvent
   {
      public ShiftList _shiftList { get; }

      public ShiftCreatedEvent(ShiftList shift)
      {
         _shiftList = shift;
      }

   }
}