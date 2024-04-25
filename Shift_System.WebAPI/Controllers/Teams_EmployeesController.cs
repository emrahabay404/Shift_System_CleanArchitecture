using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shift_System.Application.Features.Teams_Employees;
using Shift_System.Shared;
using System.Data;

namespace Shift_System.WebAPI.Controllers
{
   [Route("api/[controller]")]
   [ApiController]
   [Authorize(Roles = "Admin")]
   public class Teams_EmployeesController : ApiControllerBase
   {
      private readonly IMediator _mediator;

      public Teams_EmployeesController(IMediator mediator)
      {
         _mediator = mediator;
      }

      [HttpGet]
      public async Task<ActionResult<Result<List<GetAllTeams_EmployeesDto>>>> Get()
      {
         return await _mediator.Send(new GetAllTeams_EmployeesQuery());
      }

      [HttpPost]
      public async Task<ActionResult<Result<int>>> Create(CreateTeam_EmployeeCommand command)
      {
         return await _mediator.Send(command);
      }

      [HttpPut("{id}")]
      public async Task<ActionResult<Result<int>>> Update(int id, Update_Team_EmployeeCommand command)
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
         return await _mediator.Send(new Delete_Team_EmployeeCommand(id));
      }

   }
}
