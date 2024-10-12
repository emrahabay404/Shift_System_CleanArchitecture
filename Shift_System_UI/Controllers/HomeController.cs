using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shift_System.Application.Features.Shifts.Queries;
using Shift_System.Shared.Helpers;
using Shift_System_UI.Models;
using System.Diagnostics;

namespace Shift_System_UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMediator _mediator;

        public HomeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public IActionResult Index()
        {
            return View();
        }



        [HttpGet]
        public async Task<ActionResult<PaginatedResult<GetAllShiftsDto>>> GetShiftsWithPagination([FromQuery] GetShiftsWithPaginationQuery query)
        {
            var validator = new GetShiftsWithPaginationValidator();
            var result = validator.Validate(query);
            if (result.IsValid)
            {
                return await _mediator.Send(query);
            }
            var errorMessages = result.Errors.Select(x => x.ErrorMessage).ToList();
            return BadRequest(errorMessages);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(ErrorViewModel model)
        {
            return View(new ErrorViewModel { Message = model.Message, RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            //return View(new ErrorViewModel { Message = h,RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
