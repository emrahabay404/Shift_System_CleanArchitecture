using Microsoft.AspNetCore.Mvc;


namespace Telerik_UI.Controllers
{
   // [Authorize(Roles = "Teams")]
   public class HomeController : Controller
   {

      public IActionResult Index()
      {
         return View();
      }

   }
}
