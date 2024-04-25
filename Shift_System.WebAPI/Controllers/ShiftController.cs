using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shift_System.Application.Features.Shift;
using Shift_System.Shared;

namespace Shift_System.WebAPI.Controllers
{
   [Route("api/[controller]")]
   [ApiController]
   [Authorize(Roles = "Admin")]
   public class ShiftController : ApiControllerBase
   {
      private readonly IMediator _mediator;

      public ShiftController(IMediator mediator)
      {
         _mediator = mediator;
      }

      [HttpGet]
      public async Task<ActionResult<Result<List<GetAllShiftsDto>>>> Get()
      {
         return await _mediator.Send(new GetAllShiftsQuery());
      }

      [HttpPost]
      public async Task<ActionResult<Result<int>>> Create(CreateShiftCommand command)
      {
         return await _mediator.Send(command);
      }

      [HttpPut("{id}")]
      public async Task<ActionResult<Result<int>>> Update(int id, UpdateShiftCommand command)
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
         return await _mediator.Send(new DeleteShiftCommand(id));
      }

   }
}
