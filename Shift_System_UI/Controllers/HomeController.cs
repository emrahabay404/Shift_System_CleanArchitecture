using Application.Features.Payment;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shift_System.Application.Features.Shifts.Commands;
using Shift_System.Application.Features.Shifts.Queries;
using Shift_System.Domain.Entities.Models;
using Shift_System.Domain.Entities.Tables;
using Shift_System.Shared.Helpers;
using System.Diagnostics;

namespace Shift_System_UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMediator _mediator;
        private readonly UserManager<AppUser> _UserManager;

        public HomeController(IMediator mediator, UserManager<AppUser> userManager)
        {
            _mediator = mediator;
            _UserManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }


        //[HttpGet]
        //public IActionResult deleteshift(Guid id)
        //{
        //    return Json(false);
        //}

        [HttpGet]
        public async Task<ActionResult> Deleteshift(Guid id)
        {
            var result = await _mediator.Send(new DeleteShiftCommand(id));
            return Json(result);
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedResult<GetAllShiftsDto>>> GetShiftsWithPagination([FromQuery] GetShiftsWithPaginationQuery query)
        {
            var validator = new GetShiftsWithPaginationValidator();
            var result = validator.Validate(query);
            if (result.IsValid)
            {
                var resultdata = await _mediator.Send(query);
                return Json(resultdata);
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











        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Payment(PaymentCommand command)
        {
            command.UserId = Guid.Parse(_UserManager.GetUserAsync(User).Result.Id);
            var getframe = await _mediator.Send(command);
            ViewBag.PaymentUrl = getframe.IframeSrc;
            return View();
        }



    }
}
