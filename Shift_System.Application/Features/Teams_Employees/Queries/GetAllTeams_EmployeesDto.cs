using Shift_System.Application.Common.Mappings;
using Shift_System.Domain.Entities;

namespace Shift_System.Application.Features.Teams_Employees.Queries
{
    public class GetAllTeams_EmployeesDto : IMapFrom<TeamEmployee>
    {
        public int Id { get; init; }
        public Guid? EmployeeId { get; set; }
        public Guid? TeamId { get; set; }
        public bool? IsLeader { get; set; }
    }
}
