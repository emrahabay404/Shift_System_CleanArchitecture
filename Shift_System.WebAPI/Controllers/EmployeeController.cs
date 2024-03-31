using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shift_System.Application.Features.Employees;
using Shift_System.Shared;

namespace Shift_System.WebAPI.Controllers
{
   [Route("api/[controller]")]
   [ApiController]
   public class EmployeeController : ApiControllerBase
   {
      private readonly IMediator _mediator;

      public EmployeeController(IMediator mediator)
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




   }
}
