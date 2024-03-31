using Shift_System.Domain.Common;
using Shift_System.Domain.Entities;

namespace Shift_System.Application.Features.Employees
{
   public class EmployeeDeletedEvent : BaseEvent
   {
      public Employee _Employee { get; }

      public EmployeeDeletedEvent(Employee employee)
      {
         _Employee = employee;
      }
   }
}
