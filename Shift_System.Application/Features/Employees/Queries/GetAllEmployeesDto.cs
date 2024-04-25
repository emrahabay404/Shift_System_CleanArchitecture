using Shift_System.Application.Common.Mappings;
using Shift_System.Domain.Entities;

namespace Shift_System.Application.Features.Employees.Queries
{
    public class GetAllEmployeesDto : IMapFrom<Employee>
    {
        public int Id { get; init; }
        public int EmployeeCode { get; set; }
        public string Name { get; set; }
        public string SurName { get; set; }
        public string UserName { get; set; }
        public string Mail { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Title { get; set; }
        public bool Activity { get; set; }
    }
}
