using Shift_System.Domain.Common;
using Shift_System.Domain.Entities;

namespace Shift_System.Application.Features.Employees.Events
{
    public class EmployeeCreatedEvent : BaseEvent
    {
        public Employee _Employee { get; }

        public EmployeeCreatedEvent(Employee employee)
        {
            _Employee = employee;
        }

    }
}
