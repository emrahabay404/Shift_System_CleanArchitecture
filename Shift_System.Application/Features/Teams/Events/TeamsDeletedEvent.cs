using Shift_System.Domain.Common;
using Shift_System.Domain.Entities.Tables;

namespace Shift_System.Application.Features.Teams.Events
{
    public class TeamsDeletedEvent : BaseEvent
    {
        public Team _Team { get; }

        public TeamsDeletedEvent(Team team)
        {
            _Team = team;
        }
    }
}

