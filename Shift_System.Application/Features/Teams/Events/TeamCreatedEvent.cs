using Shift_System.Domain.Common;
using Shift_System.Domain.Entities.Tables;

namespace Shift_System.Application.Features.Teams.Events
{
    public class TeamCreatedEvent : BaseEvent
    {
        public Team _Team { get; }

        public TeamCreatedEvent(Team team)
        {
            _Team = team;
        }

    }
}
