using Shift_System.Domain.Entities;

namespace Shift_System.Application.Interfaces.Repositories
{
   public interface IEmployeeRepository
   {
      Task<List<Employee>> GetEmployeesByCodeAsync(int empCode);
   }
}
