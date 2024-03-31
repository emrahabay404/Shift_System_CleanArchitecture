using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Telerik_UI.Models;
using Telerik_UI.Models.DTOs;

namespace Telerik_UI.Controllers
{
   public class UserController : Controller
   {

      private bool _Status;
      private UserManager<AppUser> _userManager;
      private SignInManager<AppUser> _signInManager;
      

      public UserController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
      {
         _userManager = userManager;
         _signInManager = signInManager;
      }


      [AllowAnonymous]
      [Route("/Account/Login")]
      [HttpGet]
      public IActionResult Index()
      {
         return View();
      }

      [HttpPost]
      [AllowAnonymous]
      public async Task<IActionResult> Index(UserLoginDto p)
      {
         if (ModelState.IsValid)
         {
            var result = await _signInManager.PasswordSignInAsync(p.Username, p.Password, false, true);
            if (result.Succeeded)
            {

               _Status = true;
            }
            else
            {
               _Status = false;
            }
         }
         return Json(_Status);
      }


      [HttpGet]
      [AllowAnonymous]
      public IActionResult Signup() { return View(); }


      [HttpPost]
      [AllowAnonymous]
      public async Task<IActionResult> Signup(UserRegisterDto p)
      {
         if (ModelState.IsValid)
         {
            AppUser user = new()
            {
               FullName = p.NameSurname,
               Email = p.Mail,
               UserName = p.Username,
               Status = false,
            };

            var result = await _userManager.CreateAsync(user, p.Password);
            if (result.Succeeded)
            {
               _Status = true;
            }
            else
            {
               foreach (var item in result.Errors)
               {
                  ModelState.AddModelError("", item.Description);
               }
            }
         }
         else
         {
            _Status = false;
         }
         return Json(_Status);
      }




      [HttpGet]
      [AllowAnonymous]
      public async Task<IActionResult> Logout()
      {
         await _signInManager.SignOutAsync();
         return RedirectToAction("Index", "User");
      }

      [AllowAnonymous]
      [Route("/Account/AccessDenied")]
      public IActionResult PageDenied()
      {
         return View();
      }

      [AllowAnonymous]
      public IActionResult Page404()
      {
         return View();
      }

   }
}
