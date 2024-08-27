using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shift_System.Application.Features.Employees.Commands;
using Shift_System.Application.Features.Employees.Queries;
using Shift_System.Shared.Helpers;

namespace Micro.Employees.API.Controllers
{
   [Route("api/[controller]")]
   [ApiController]
   public class EmployeesController : ControllerBase
   {
      private readonly IMediator _mediator;

      public EmployeesController(IMediator mediator)
      {
         _mediator = mediator;
      }

      [HttpGet]
      public async Task<ActionResult<Result<List<GetAllEmployeesDto>>>> Get()
      {
         return await _mediator.Send(new GetAllEmployeesQuery());
      }

      [HttpPost]
      public async Task<ActionResult<Result<int>>> Create(CreateEmployeeCommand command)
      {
         return await _mediator.Send(command);
      }

      [HttpPut("{id}")]
      public async Task<ActionResult<Result<int>>> Update(int id, UpdateEmployeeCommand command)
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
         return await _mediator.Send(new DeleteEmployeeCommand(id));
      }

   }
}
