using Shift_System.Application.Features.Teams_Employees.Commands;

namespace Shift_System.Application.Interfaces.Repositories
{
    public interface ITeam_EmployeeRepository
   {

      int Team_Leader_Check(CreateTeam_EmployeeCommand command);
      int One_Team_Check(CreateTeam_EmployeeCommand command);
      /// ////

      int Team_Leader_Check_2(Update_Team_EmployeeCommand command);
      int One_Team_Check_2(Update_Team_EmployeeCommand command);


   }
}
