using Shift_System.Domain.Common;
using Shift_System.Domain.Entities.Tables;

namespace Shift_System.Application.Features.Employees.Events
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
