using Shift_System.Domain.Entities.Tables;

namespace Shift_System.Application.Interfaces.Repositories
{
    public interface IEmployeeRepository
   {
      Task<List<Employee>> GetEmployeesByCodeAsync(int empCode);
   }
}
