using Shift_System.Domain.Common;
using Shift_System.Domain.Entities;

namespace Shift_System.Application.Features.Assignments.Events
{
    public class AssignCreatedEvent : BaseEvent
    {
        public AssignList _assignList { get; }

        public AssignCreatedEvent(AssignList assignList)
        {
            _assignList = assignList;
        }

    }
}
