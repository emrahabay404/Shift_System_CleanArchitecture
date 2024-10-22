using Shift_System.Application.Common.Mappings;
using Shift_System.Domain.Entities.Tables;

namespace Shift_System.Application.Features.Assignments.Queries
{
    public class GetAllAssignsDto : IMapFrom<AssignList>
    {
        public Guid Id { get; init; }

        public Guid? ShiftId { get; set; }
        public Guid? TeamId { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool Status{ get; set; }

    }
}
