using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shift_System.Application.Features.Employees.Commands;
using Shift_System.Application.Features.Employees.Queries;

namespace Telerik_UI.Controllers
{
   // [Authorize(Roles = "Employee")]
   public class EmployeeController : Controller
   {

      private readonly ILogger<EmployeeController> _logger;
      private readonly IMediator _mediator;
      public EmployeeController(ILogger<EmployeeController> logger, IMediator mediator)
      {
         _logger = logger; _mediator = mediator;
      }


      [HttpGet]
      public IActionResult Index()
      {
         return View();
      }

      public ActionResult GetEmployees([DataSourceRequest] DataSourceRequest request)
      {
         var _teams = _mediator.Send(new GetAllEmployeesQuery());
         var dsResult = _teams.Result.Data.ToDataSourceResult(request);
         return Json(dsResult);
      }

      [HttpPost]
      public ActionResult Create(CreateEmployeeCommand command)
      {
         var _create = _mediator.Send(command);
         return Ok(_create.Result);
      }

      //[HttpPut("{id}")]
      public ActionResult Update(int id, UpdateEmployeeCommand command)
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
         var abc = _mediator.Send(new DeleteEmployeeCommand(id));
         return Ok(abc.Result);
      }

   }
}
