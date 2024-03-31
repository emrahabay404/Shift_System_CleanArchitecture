using Shift_System.Domain.Common;
using Shift_System.Domain.Entities;

namespace Shift_System.Application.Features.Shift
{
   public class ShiftListDeletedEvent : BaseEvent
   {
      public ShiftList _shiftList { get; }

      public ShiftListDeletedEvent(ShiftList shiftList)
      {
         _shiftList = shiftList;
      }
   }
}

