using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shift_System.Application.Features.Shifts.Queries;
using Shift_System.Shared.Helpers;

namespace Shift_System.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Manager,Admin")]
    public class AdminController : ApiControllerBase
    {
        private readonly IMediator _mediator;
        public AdminController(IMediator mediator)
        {
            _mediator = mediator;
        }



        [HttpPost("GetShiftsWithPagination")]
        [AllowAnonymous]
        public async Task<ActionResult<PaginatedResult<GetAllShiftsDto>>> GetShiftsWithPagination([FromBody] DynamicQuery query)
        {
            var request = new GetShiftsDinamik(query);
            var resultData = await _mediator.Send(request);
            return Ok(resultData);
        }


    }
}
