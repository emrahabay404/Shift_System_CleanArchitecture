using Shift_System.Domain.Common;
using Shift_System.Domain.Entities;

namespace Shift_System.Application.Features.Assignments.Events
{
    public class AssignUpdatedEvent : BaseEvent
    {
        public AssignList _assignList { get; }

        public AssignUpdatedEvent(AssignList assignList)
        {
            _assignList = assignList;
        }
    }
}
