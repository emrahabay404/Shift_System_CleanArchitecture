using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shift_System.Application.Features.Teams_Employees;
using Shift_System.Application.Features.Teams_Employees.Commands;

namespace Telerik_UI.Controllers
{
    // [Authorize(Roles = "TeamEmployee")]
    public class TeamEmployeeController : Controller
   {

      private readonly ILogger<TeamEmployeeController> _logger;
      private readonly IMediator _mediator;
      public TeamEmployeeController(ILogger<TeamEmployeeController> logger, IMediator mediator)
      {
         _logger = logger; _mediator = mediator;
      }

      [HttpGet]
      public IActionResult Index()
      {
         return View();
      }

      public ActionResult GetTeamEmployee([DataSourceRequest] DataSourceRequest request)
      {
         var _teams = _mediator.Send(new GetAllTeams_EmployeesQuery());
         var dsResult = _teams.Result.Data.ToDataSourceResult(request);
         return Json(dsResult);
      }

      [HttpPost]
      public ActionResult Create(CreateTeam_EmployeeCommand command)
      {
         var _create = _mediator.Send(command);
         return Json(_create.Result.Messages);
      }

      //[HttpPut("{id}")]
      public ActionResult Update(int id, Update_Team_EmployeeCommand command)
      {
         if (id != command.Id)
         {
            return BadRequest();
         }
         var _update = _mediator.Send(command);
         return Json(_update.Result.Messages);
      }

      //[HttpDelete("{id}")]
      public ActionResult Destroy(int id)
      {
         var abc = _mediator.Send(new Delete_Team_EmployeeCommand(id));
         return Ok(abc.Result);
      }

   }
}
