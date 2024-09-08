using Shift_System.Application.Common.Mappings;
using Shift_System.Domain.Entities;

namespace Shift_System.Application.Features.Shifts.Queries
{
    public class GetAllShiftsDto : IMapFrom<ShiftList>
    {
        public int Id { get; init; }
        public string Shift_Name { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedDate { get; set; }
        public string Url1 { get; set; }
        public string Url2 { get; set; }
        public string Url3 { get; set; }
        public string Url4 { get; set; }
        public string Url5 { get; set; }
    }
}
