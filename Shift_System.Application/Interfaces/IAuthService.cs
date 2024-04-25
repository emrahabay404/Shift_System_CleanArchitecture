using Shift_System.Domain.Entities.Models;

namespace Shift_System.Application.Interfaces
{
   public interface IAuthService
   {
      Task<(int, string)> Registeration(RegistrationModel model, string role);
      Task<(int, string)> Login(LoginModel model);
   }
}

