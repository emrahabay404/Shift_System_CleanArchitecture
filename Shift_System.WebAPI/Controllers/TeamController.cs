﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shift_System.Application.Features.Teams;
using Shift_System.Shared;

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


   }
}
