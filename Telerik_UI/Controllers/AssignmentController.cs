using Azure.Core;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shift_System.Application.Features.Assignments;
using Shift_System.Domain.Entities;

namespace Telerik_UI.Controllers
{

   // [Authorize(Roles = "Assignment")]
   public class AssignmentController : Controller
   {
      private readonly ILogger<AssignmentController> _logger;
      private readonly IMediator _mediator;
      public AssignmentController(ILogger<AssignmentController> logger, IMediator mediator)
      {
         _logger = logger;
         _mediator = mediator;
      }


      [HttpGet]
      public IActionResult Index()
      {
         return View();
      }

      public ActionResult GetAssignments([DataSourceRequest] DataSourceRequest request)
      {
         var _teams = _mediator.Send(new GetAllAssignsQuery());
         var dsResult = _teams.Result.Data.ToDataSourceResult(request);
         return Json(dsResult);
      }

      [HttpPost]
      public ActionResult Create(CreateAssignCommand command)
      {
         var _create = _mediator.Send(command);
         return Ok(_create.Result);
      }

      //[HttpPut("{id}")]
      public ActionResult Update(int id, UpdateAssignmentCommand command)
      {
         if (id != command.Id)
         {
            return BadRequest();
         }
         var _update = _mediator.Send(command);
         return Ok(_update.Result);
      }

      //[HttpDelete("{id}")]
      public ActionResult Delete(int id)
      {
         var abc = _mediator.Send(new DeleteAssignmentCommand(id));
         return Ok(abc.Result);
      }




   }
}
