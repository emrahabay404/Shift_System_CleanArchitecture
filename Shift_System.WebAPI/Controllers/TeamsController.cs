using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shift_System.Application.Features.Teams.Queries;
using Shift_System.Shared.Helpers;

namespace Shift_System.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : ApiControllerBase
    {
        private readonly IMediator _mediator;
        //private readonly ILogger<AuthController> _logger;

        public TeamsController(IMediator mediator)
        {
            _mediator = mediator;
        }



        [HttpGet]
        [Authorize(Roles = "Adminn")]
        public async Task<ActionResult<Result<List<GetAllTeamsDto>>>> GetTeams()
        {
            return await _mediator.Send(new GetAllTeamsQuery());
        }



    }
}
