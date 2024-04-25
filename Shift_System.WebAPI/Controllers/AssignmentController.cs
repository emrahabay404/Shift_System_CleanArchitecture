using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shift_System.Application.Features.Assignments;
using Shift_System.Application.Features.Assignments.Commands;
using Shift_System.Application.Features.Assignments.Queries;
using Shift_System.Shared;

namespace Shift_System.WebAPI.Controllers
{
    [Route("api/[controller]")]
   [ApiController]
   [Authorize(Roles = "Admin")]
   public class AssignmentController : ApiControllerBase
   {
      private readonly IMediator _mediator;

      public AssignmentController(IMediator mediator)
      {
         _mediator = mediator;
      }

      [HttpGet]
      public async Task<ActionResult<Result<List<GetAllAssignsDto>>>> Get()
      {
         return await _mediator.Send(new GetAllAssignsQuery());
      }

      [HttpPost]
      public async Task<ActionResult<Result<int>>> Create(CreateAssignCommand command)
      {
         return await _mediator.Send(command);
      }

      [HttpPut("{id}")]
      public async Task<ActionResult<Result<int>>> Update(int id, UpdateAssignmentCommand command)
      {
         if (id != command.Id)
         {
            return BadRequest();
         }
         return await _mediator.Send(command);
      }

      [HttpDelete("{id}")]
      public async Task<ActionResult<Result<int>>> Delete(int id)
      {
         return await _mediator.Send(new DeleteAssignmentCommand(id));
      }

   }
}
