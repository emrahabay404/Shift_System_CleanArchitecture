using Microsoft.AspNetCore.Mvc;
using Shift_System.Infrastructure.Services;

namespace Shift_System.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : ApiControllerBase
    {
        private readonly LoggedUserService _loggedUserService;
        public TeamsController(LoggedUserService loggedUserService)
        {
            _loggedUserService = loggedUserService;
        }


        [HttpGet]
        public IActionResult Getuser()
        {
            var a = _loggedUserService.GetUserNameApi();
            var b = _loggedUserService.GetUserIdApi();
            var c = _loggedUserService.GetUserEmailApi();
            var d = _loggedUserService.GetUserFullNameApi();
            return Ok(new { a, b, c,d });
        }

    }
}