using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shift_System.Application.Features.Teams.Queries;
using Shift_System.Shared;
using Shift_System_UI.Models;
using System.Diagnostics;

namespace Shift_System_UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult<List<GetAllTeamsDto>> Index()
        {
            return View(_mediator.Send(new GetAllTeamsQuery()).Result.Data);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
