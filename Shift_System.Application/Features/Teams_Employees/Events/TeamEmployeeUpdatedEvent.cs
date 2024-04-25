using Shift_System.Domain.Common;
using Shift_System.Domain.Entities;

namespace Shift_System.Application.Features.Teams_Employees.Events
{
    public class TeamEmployeeUpdatedEvent : BaseEvent
    {
        public TeamEmployee _teamEmployee { get; }

        public TeamEmployeeUpdatedEvent(TeamEmployee teamEmployee)
        {
            _teamEmployee = teamEmployee;
        }

    }
}
