using Microsoft.EntityFrameworkCore;
using Shift_System.Application.Interfaces.Repositories;
using Shift_System.Domain.Entities;

namespace Shift_System.Persistence.Repositories
{
   public class EmployeeRepository : IEmployeeRepository
   {
      private readonly IGenericRepository<Employee> _repository;

      public EmployeeRepository(IGenericRepository<Employee> repository)
      {
         _repository = repository;
      }

      public async Task<List<Employee>> GetEmployeesByCodeAsync(int clubId)
      {
         return await _repository.Entities.Where(x => x.EmployeeCode == clubId).ToListAsync();
      }

   }
}
