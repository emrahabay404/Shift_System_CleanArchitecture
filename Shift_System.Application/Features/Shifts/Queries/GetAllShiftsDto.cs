using Shift_System.Application.Common.Mappings;
using Shift_System.Domain.Entities.Tables;

namespace Shift_System.Application.Features.Shifts.Queries
{
    public class GetAllShiftsDto : IMapFrom<ShiftList>
    {
        public Guid Id { get; init; }
        public string Shift_Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool Status { get; set; }
        public bool IsDeleted { get; set; }
    }
}
