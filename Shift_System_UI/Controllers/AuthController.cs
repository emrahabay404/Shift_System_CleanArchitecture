using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shift_System.Application.Interfaces;
using Shift_System.Domain.Entities;

namespace Shift_System_UI.Controllers
{
    [AllowAnonymous]
    public class AuthController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IAuthService _authService;
        public AuthController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IAuthService authService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _authService = authService;
        }

        [HttpGet]
        [Route("/Account/Login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> Login(string username, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(username, password, true, true);
            if (result.Succeeded)
            {
                return Json(true);
                //var user = await _userManager.FindByNameAsync(username);
                //if (user.EmailConfirmed == true){}
            }
            else
            {
                return Json(false);
            }
        }


        [HttpGet]
        [Authorize]
        public IActionResult Logout()
        {
            var result = _signInManager.SignOutAsync();
            if (result.IsCompleted)
            {
                return Json(true);
            }
            return Json(false);
        }

    }
}
