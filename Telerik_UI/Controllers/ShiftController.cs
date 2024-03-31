using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shift_System.Application.Features.Shift;

namespace Telerik_UI.Controllers
{
   // [Authorize(Roles = "Shifts")]
   public class ShiftController : Controller
   {

      private readonly ILogger<ShiftController> _logger;
      private readonly IMediator _mediator;
      public ShiftController(ILogger<ShiftController> logger, IMediator mediator)
      {
         _logger = logger; _mediator = mediator;
      }

      [HttpGet]
      public IActionResult Index()
      {
         return View();
      }

      public ActionResult GetShifts([DataSourceRequest] DataSourceRequest request)
      {
         var _teams = _mediator.Send(new GetAllShiftsQuery());
         var dsResult = _teams.Result.Data.ToDataSourceResult(request);
         return Json(dsResult);
      }

      [HttpPost]
      public ActionResult Create(CreateShiftCommand command)
      {
         var _create = _mediator.Send(command);
         return Ok(_create.Result);
      }

      //[HttpPut("{id}")]
      public ActionResult Update(int id, UpdateShiftCommand command)
      {
         if (id != command.Id)
         {
            return BadRequest();
         }
         var _update = _mediator.Send(command);
         return Ok(_update.Result);
      }

      //[HttpDelete("{id}")]
      public ActionResult Destroy(int id)
      {
         var abc = _mediator.Send(new DeleteShiftCommand(id));
         return Ok(abc.Result);
      }


   }
}
