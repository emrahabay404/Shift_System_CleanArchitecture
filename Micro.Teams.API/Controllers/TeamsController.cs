using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shift_System.Application.Features.Shifts.Queries;
using Shift_System.Application.Features.Teams.Commands;
using Shift_System.Application.Features.Teams.Queries;
using Shift_System.Shared.Helpers;

namespace Micro.Teams.API.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TeamsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetTeams")]
        [AllowAnonymous]
        public async Task<ActionResult<Result<List<GetAllTeamsDto>>>> GetTeams()
        {
            return await _mediator.Send(new GetAllTeamsQuery());
        }

        [HttpGet("paged")]
        [AllowAnonymous]
        public async Task<ActionResult<PaginatedResult<GetAllShiftsDto>>> GetShiftsWithPagination([FromQuery] GetShiftsWithPaginationQuery query)
        {
            var validator = new GetShiftsWithPaginationValidator();
            var result = validator.Validate(query);
            if (result.IsValid)
            {
                return await _mediator.Send(query);
            }
            var errorMessages = result.Errors.Select(x => x.ErrorMessage).ToList();
            return BadRequest(errorMessages);
        }

        [HttpPost]
        public async Task<ActionResult<Result<Guid>>> Create(CreateTeamCommand command)
        {
            return await _mediator.Send(command);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Result<Guid>>> Update(Guid id, UpdateTeamsCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            return await _mediator.Send(command);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Result<Guid>>> Delete(Guid id)
        {
            return await _mediator.Send(new DeleteTeamsCommand(id));
        }

    }
}
