using Application.Features.Payment;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shift_System.Application.Features.Assignments.Queries;
using Shift_System.Shared.Helpers;
using System.Security.Claims;

namespace Shift_System.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ApiControllerBase
    {
        private readonly IMediator _Mediator;
        public PaymentController(IMediator mediator)
        {
            _Mediator = mediator;
        }

        [Authorize]
        [HttpPost("MakePay")]
        public async Task<IActionResult> MakePay(PaymentCommand command)
        {
            var _UserId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            command.UserId = Guid.Parse(_UserId);
            var response = await _Mediator.Send(command);
            return Ok(response);
        }



        [AllowAnonymous]
        [HttpPost("Dinamik")]
        public async Task<IActionResult> Dinamik([FromBody] DynamicQuery query)
        {
            query ??= new DynamicQuery();
            var result = await _Mediator.Send(new GetAllAssignsQueryDynamic(query));
            if (result.Succeeded)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }





    }
}
