using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Telerik_UI.Models;
using Telerik_UI.Models.DTOs;

namespace Telerik_UI.Controllers
{
   //[Authorize(Roles = "admin,manager")]
   public class AdminController : Controller
   {

      private readonly RoleManager<AppRole> _roleManager;
      private readonly UserManager<AppUser> _userManager;

      public AdminController(RoleManager<AppRole> roleManager, UserManager<AppUser> userManager)
      {
         _roleManager = roleManager;
         _userManager = userManager;
      }
      public IActionResult Home()
      {

         return View();
      }

      public IActionResult Index()
      {
         var values = _roleManager.Roles.ToList();
         return View(values);
      }

      [HttpGet]
      public IActionResult AddRole()
      {
         return View();
      }
      [HttpPost]
      public async Task<IActionResult> AddRole(RoleViewModel p)
      {
         if (ModelState.IsValid)
         {
            AppRole appRole = new()
            {
               Name = p.Name
            };
            var result = await _roleManager.CreateAsync(appRole);
            if (result.Succeeded)
            {
               return RedirectToAction("Index");
            }
            else
            {
               foreach (var item in result.Errors)
               {
                  ModelState.AddModelError("", item.Description);
               }
            }
         }
         return View();
      }
      //ROLE TABLOSUNDA KÜÇÜK HARFLİ OLAN BİRDE BÜYÜK HARFLE NORMALİZE OLARAK
      //BÜYÜK TÜRDEN EKLENECEK.

      [HttpGet]
      public IActionResult UpdateRole(int id)
      {
         var values = _roleManager.Roles.FirstOrDefault(x => x.Id == id);
         RoleUpdateViewModel model = new()
         {
            Id = values.Id,
            Name = values.Name
         };
         return View(model);
      }

      [HttpPost]
      public async Task<IActionResult> UpdateRole(RoleUpdateViewModel model)
      {
         var values = _roleManager.Roles.Where(x => x.Id == model.Id).FirstOrDefault();
         values.Name = model.Name;
         var result = await _roleManager.UpdateAsync(values);
         if (result.Succeeded)
         {
            return RedirectToAction("Index");
         }
         return View(model);
      }

      public async Task<IActionResult> DeleteRole(int id)
      {
         var values = _roleManager.Roles.FirstOrDefault(x => x.Id == id);
         var result = await _roleManager.DeleteAsync(values);
         if (result.Succeeded)
         {
            return RedirectToAction("Index");
         }
         return View();
      }

      public IActionResult UserRoleList()
      {
         var values = _userManager.Users.ToList();
         return View(values);
      }

      [HttpGet]
      public async Task<IActionResult> AssignRole(int id)
      {
         var users = _userManager.Users.FirstOrDefault(x => x.Id == id);
         var roles = _roleManager.Roles.ToList();
         TempData["UserId"] = users.Id;
         var userroles = await _userManager.GetRolesAsync(users);

         List<RoleAssignViewModel> model = new();
         foreach (var item in roles)
         {
            RoleAssignViewModel m = new();
            m.RoleId = item.Id;
            m.Name = item.Name;
            m.Exists = userroles.Contains(item.Name);
            model.Add(m);
         }

         return View(model);
      }

      [HttpPost]
      public async Task<IActionResult> AssignRole(List<RoleAssignViewModel> model)
      {
         var userid = (int)TempData["UserId"];
         var user = _userManager.Users.FirstOrDefault(x => x.Id == userid);
         foreach (var item in model)
         {
            if (item.Exists)
            {
               await _userManager.AddToRoleAsync(user, item.Name);

            }
            else
            {
               await _userManager.RemoveFromRoleAsync(user, item.Name);
            }
         }
         return RedirectToAction("UserRoleList");
      }

   }
}
