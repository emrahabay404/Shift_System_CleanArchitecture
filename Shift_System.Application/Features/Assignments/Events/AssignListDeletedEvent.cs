using Shift_System.Domain.Common;
using Shift_System.Domain.Entities.Tables;

namespace Shift_System.Application.Features.Assignments.Events
{
    public class AssignListDeletedEvent : BaseEvent
    {
        public AssignList _assignList { get; }

        public AssignListDeletedEvent(AssignList assignList)
        {
            _assignList = assignList;
        }
    }
}
