﻿using Shift_System.Domain.Common;
using Shift_System.Domain.Entities.Tables;

namespace Shift_System.Application.Features.Teams_Employees.Events
{
    public class Team_EmployeeDeletedEvent : BaseEvent
    {
        public TeamEmployee _teamEmployee { get; }

        public Team_EmployeeDeletedEvent(TeamEmployee teamEmployee)
        {
            _teamEmployee = teamEmployee;
        }

    }
}
