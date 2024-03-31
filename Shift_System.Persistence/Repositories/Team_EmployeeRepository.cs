using Shift_System.Application.Features.Teams_Employees;
using Shift_System.Application.Interfaces.Repositories;
using Shift_System.Domain.Entities;

namespace Shift_System.Persistence.Repositories
{
   public class Team_EmployeeRepository : ITeam_EmployeeRepository
   {
      private readonly IGenericRepository<TeamEmployee> _repository;

      public Team_EmployeeRepository(IGenericRepository<TeamEmployee> repository)
      {
         _repository = repository;
      }

      public int One_Team_Check(CreateTeam_EmployeeCommand command)
      {
         return _repository.Entities.Where(x => x.EmployeeId == command.EmployeeId && x.TeamId == command.TeamId).Count();
      }

      public int One_Team_Check_2(Update_Team_EmployeeCommand command)
      {
         return _repository.Entities.Where(x => x.EmployeeId == command.EmployeeId && x.TeamId == command.TeamId).Count();
      }

      public int Team_Leader_Check(CreateTeam_EmployeeCommand command)
      {
         return _repository.Entities.Where(x => x.EmployeeId == command.EmployeeId && x.IsLeader == true).Count();
      }

      public int Team_Leader_Check_2(Update_Team_EmployeeCommand command)
      {
         return _repository.Entities.Where(x => x.EmployeeId == command.EmployeeId && x.IsLeader == true).Count();
      }

   }
}
