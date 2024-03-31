using Shift_System.Application.DTOs.Email;

namespace Shift_System.Application.Interfaces
{
   public interface IEmailService
   {
      Task SendAsync(EmailRequestDto request);
   }
}
