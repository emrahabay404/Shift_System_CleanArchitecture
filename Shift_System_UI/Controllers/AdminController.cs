using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shift_System.Domain.Entities;
using Shift_System.Shared.Helpers; // Messages sınıfını kullanmak için ekliyoruz
using Shift_System_UI.Models;

namespace Shift_System_UI.Controllers
{
    [Authorize(Roles = "Admin")]
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
            // Kullanıcılar ve onların rollerini içeren bir model oluşturuyoruz
            var model = new RoleAssignmentViewModel
            {
                Users = _userManager.Users.ToList(),
                Roles = _roleManager.Roles.ToList(),
                UserRoles = new List<UserRolesViewModel>()
            };

            // Kullanıcıların rolleri için bir liste dolduruyoruz
            foreach (var user in model.Users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                model.UserRoles.Add(new UserRolesViewModel
                {
                    Username = user.UserName,
                    FullName = user.FullName,      // FullName burada alınıyor
                    Email = user.Email,            // Email burada alınıyor
                    PhoneNumber = user.PhoneNumber, // PhoneNumber burada alınıyor
                    Roles = roles.ToList()
                });
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult CreateUser()
        {
            // Roller veritabanından alınıyor
            var roles = _roleManager.Roles.ToList();
            if (roles == null || roles.Count == 0)
            {
                // Eğer rol yoksa uyarı mesajı
                TempData["Message"] = "Hiç rol bulunamadı. Lütfen önce rol ekleyin.";
                return RedirectToAction("Page");
            }

            var model = new CreateUserViewModel
            {
                Roles = roles // Rolleri modele ekliyoruz
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Roles = _roleManager.Roles.ToList();
                TempData["Message"] = Messages.Invalid_Input_TR;
                return View(model);
            }

            var user = new AppUser
            {
                UserName = model.Username,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,  // Cep telefonu bilgisi burada ekleniyor
                FullName = model.FullName // Önceki eklemede FullName de var
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(model.RoleName))
                {
                    var roleExists = await _roleManager.RoleExistsAsync(model.RoleName);
                    if (roleExists)
                    {
                        await _userManager.AddToRoleAsync(user, model.RoleName);
                        TempData["Message"] = Messages.User_Registered_Successfully_TR + " ve rol başarıyla atandı.";
                    }
                    else
                    {
                        TempData["Message"] = Messages.Role_Not_Found_TR;
                    }
                }
                else
                {
                    TempData["Message"] = Messages.User_Registered_Successfully_TR;
                }

                return RedirectToAction("Page");
            }
            else
            {
                model.Roles = _roleManager.Roles.ToList();
                TempData["Message"] = Messages.User_Registration_Failed_TR;
                foreach (var error in result.Errors)
                {
                    TempData["Message"] += error.Description;
                }
                return View(model);
            }
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

                TempData["Message"] = Messages.Invalid_Input_TR; // "Geçersiz model verisi!" yerine Messages sınıfı kullanıldı
                return RedirectToAction("Page");
            }

            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null)
            {
                TempData["Message"] = Messages.User_Not_Found_TR; // Kullanıcı bulunamadı mesajı
                return RedirectToAction("Page");
            }

            var roleExists = await _roleManager.RoleExistsAsync(model.RoleName);
            if (!roleExists)
            {
                TempData["Message"] = Messages.Role_Not_Found_TR; // Rol bulunamadı mesajı
                return RedirectToAction("Page");
            }

            var result = await _userManager.AddToRoleAsync(user, model.RoleName);
            if (result.Succeeded)
            {
                TempData["Message"] = Messages.Role_Assigned_Successfully_TR; // Rol başarıyla atandı mesajı
            }
            else
            {
                TempData["Message"] = Messages.Role_Assignment_Failed_TR; // Rol atama başarısız mesajı
            }

            return RedirectToAction("Page");
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
            {
                TempData["Message"] = Messages.Invalid_Input_TR; // Geçersiz giriş mesajı
                return RedirectToAction("Page");
            }

            var roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (roleExists)
            {
                TempData["Message"] = Messages.Role_Already_Exists_TR; // Rol zaten var mesajı
                return RedirectToAction("Page");
            }

            var result = await _roleManager.CreateAsync(new IdentityRole(roleName));
            if (result.Succeeded)
            {
                TempData["Message"] = Messages.Role_Created_Successfully_TR; // Rol başarıyla oluşturuldu mesajı
            }
            else
            {
                TempData["Message"] = Messages.Role_Creation_Failed_TR; // Rol oluşturma başarısız mesajı
            }

            return RedirectToAction("Page");
        }

    }
}
