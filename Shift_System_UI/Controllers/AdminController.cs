using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Shift_System_UI.Controllers
{
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class AdminController : Controller
    {
        public IActionResult Page()
        {
            return View();
        }





    }
}
