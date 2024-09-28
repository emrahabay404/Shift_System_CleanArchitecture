using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shift_System.Domain.Entities;
using Shift_System_UI.Models;

namespace Shift_System_UI.Controllers
{
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public async Task<IActionResult> Page()
        {
            // Kullanıcı ve rollerin listelerini modelde hazırlıyoruz
            var model = new RoleAssignmentViewModel
            {
                Users = _userManager.Users.ToList(),
                Roles = _roleManager.Roles.ToList()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AssignRole(RoleAssignmentModel model)
        {
            if (!ModelState.IsValid)
            {
                // ModelState içindeki hataları loglayın veya hata mesajını gösterin
                foreach (var modelError in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(modelError.ErrorMessage); // Hata mesajını konsola yazdırabilirsiniz
                }

                TempData["Message"] = "Geçersiz model verisi!";
                return RedirectToAction("Page");
            }

            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null)
            {
                TempData["Message"] = "Kullanıcı bulunamadı!";
                return RedirectToAction("Page");
            }

            var roleExists = await _roleManager.RoleExistsAsync(model.RoleName);
            if (!roleExists)
            {
                TempData["Message"] = "Rol bulunamadı!";
                return RedirectToAction("Page");
            }

            var result = await _userManager.AddToRoleAsync(user, model.RoleName);
            if (result.Succeeded)
            {
                TempData["Message"] = "Rol başarıyla kullanıcıya atandı.";
            }
            else
            {
                TempData["Message"] = "Rol atama başarısız oldu.";
            }

            return RedirectToAction("Page");
        }

    }
}