using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Shift_System.Application.Features.Shift;
using Shift_System.Shared;

namespace Shift_System.WebAPI.Controllers
{
   [Route("api/[controller]")]
   [ApiController]
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





   }
}
