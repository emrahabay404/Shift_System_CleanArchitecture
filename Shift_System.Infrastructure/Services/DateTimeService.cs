using Shift_System.Application.Interfaces;

namespace Shift_System.Infrastructure.Services
{
   public class DateTimeService : IDateTimeService
   {
      public DateTime NowUtc => DateTime.UtcNow;
   }
}