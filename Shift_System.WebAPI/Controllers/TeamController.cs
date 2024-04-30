using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shift_System.Application.Features.Teams.Commands;
using Shift_System.Application.Features.Teams.Queries;
using Shift_System.Shared;
using System.Data;

namespace Shift_System.WebAPI.Controllers
{
   [Route("api/[controller]")]
   [ApiController] 
   public class TeamController : ApiControllerBase
   {

      private readonly IMediator _mediator;

      public TeamController(IMediator mediator)
      {
         _mediator = mediator;
      }

      [HttpGet]
      public async Task<ActionResult<Result<List<GetAllTeamsDto>>>> Get()
      {
         return await _mediator.Send(new GetAllTeamsQuery());
      }

      [HttpPost]
      public async Task<ActionResult<Result<int>>> Create(CreateTeamCommand command)
      {
         return await _mediator.Send(command);
      }

      [HttpPut("{id}")]
      public async Task<ActionResult<Result<int>>> Update(int id, UpdateTeamsCommand command)
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
         return await _mediator.Send(new DeleteTeamsCommand(id));
      }

   }
}
