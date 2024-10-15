using Shift_System.Application.Common.Mappings;
using Shift_System.Domain.Entities.Tables;

namespace Shift_System.Application.Features.Employees.Queries
{
    public class GetAllEmployeesDto : IMapFrom<Employee>
    {
        public int Id { get; init; }
        public int EmployeeCode { get; set; }
        public string Name { get; set; } = string.Empty;
        public string SurName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Mail { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public bool Activity { get; set; }
    }
}
