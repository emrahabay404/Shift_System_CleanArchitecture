using Shift_System.Application.Common.Mappings;
using Shift_System.Domain.Entities;

namespace Shift_System.Application.Features.Teams_Employees.Queries
{
    public class GetAllTeams_EmployeesDto : IMapFrom<TeamEmployee>
    {
        public int Id { get; init; }
        public int? EmployeeId { get; set; }
        public int? TeamId { get; set; }
        public bool? IsLeader { get; set; }
    }
}
