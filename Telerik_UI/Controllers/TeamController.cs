using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shift_System.Application.Features.Teams.Commands;
using Shift_System.Application.Features.Teams.Queries;

namespace Telerik_UI.Controllers
{
   public class TeamController : Controller
   {

      private readonly ILogger<TeamController> _logger;
      private readonly IMediator _mediator;
      public TeamController(ILogger<TeamController> logger, IMediator mediator)
      {
         _logger = logger; _mediator = mediator;
      }


      [HttpGet]
      public IActionResult Index()
      {
         return View();
      }

      public ActionResult GetTeams([DataSourceRequest] DataSourceRequest request)
      {
         var _teams = _mediator.Send(new GetAllTeamsQuery());
         var dsResult = _teams.Result.Data.ToDataSourceResult(request);
         return Json(dsResult);
      }

      [HttpPost]
      public ActionResult Create(CreateTeamCommand command)
      {
         var _create = _mediator.Send(command);
         return Ok(_create.Result);
      }

      //[HttpPut("{id}")]
      public ActionResult Update(int id, UpdateTeamsCommand command)
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
         var abc = _mediator.Send(new DeleteTeamsCommand(id));
         return Ok(abc.Result);
      }


   }
}
