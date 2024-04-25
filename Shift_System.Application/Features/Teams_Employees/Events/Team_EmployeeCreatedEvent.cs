using Shift_System.Domain.Common;
using Shift_System.Domain.Entities;

namespace Shift_System.Application.Features.Teams_Employees.Events
{
    public class Team_EmployeeCreatedEvent : BaseEvent
    {
        public TeamEmployee _teamEmployee { get; }

        public Team_EmployeeCreatedEvent(TeamEmployee teamEmployee)
        {
            _teamEmployee = teamEmployee;
        }

    }
}
