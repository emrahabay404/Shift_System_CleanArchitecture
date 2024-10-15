using Application.Features.Payment;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

    }
}
