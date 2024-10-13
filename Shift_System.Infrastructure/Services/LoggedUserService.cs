using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Shift_System.Infrastructure.Services
{
    public class LoggedUserService
    {

        private readonly IHttpContextAccessor _HttpContextAccessor;

        public LoggedUserService(IHttpContextAccessor httpContextAccessor)
        {

            _HttpContextAccessor = httpContextAccessor;
        }


        /////API
        public string GetUserIdApi()
        {
            return _HttpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public string GetUserNameApi()
        {
            return _HttpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);
        }

        public string GetUserEmailApi()
        {
            return _HttpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email);
        }
        public string GetUserFullNameApi()
        {
            return _HttpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.GivenName);
        }



    }
}