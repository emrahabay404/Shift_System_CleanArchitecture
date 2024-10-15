using Shift_System.Domain.Common;
using Shift_System.Domain.Entities.Tables;

namespace Shift_System.Application.Features.Teams.Events
{
    public class TeamUpdatedEvent : BaseEvent
    {
        public Team _Team { get; }

        public TeamUpdatedEvent(Team team)
        {
            _Team = team;
        }
    }
}
