using Shift_System.Domain.Common;
using Shift_System.Domain.Entities;

namespace Shift_System.Application.Features.Teams
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
