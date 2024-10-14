using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shift_System.Application.Features.Documents.Commands;
using Shift_System.Application.Features.Teams.Commands;
using Shift_System.Application.Interfaces;
using Shift_System.Application.Interfaces.Repositories;
using Shift_System.Infrastructure.Services;
using Shift_System.Shared.Helpers;

namespace Shift_System.WebAPI.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        private readonly IFileUploadService _fileUploadService;
        private readonly IMediator _mediator;
        private readonly LoggedUserService _loggedUserService;

        public FileUploadController(IMediator mediator, IFileUploadService fileUploadService, LoggedUserService loggedUserService)
        {
            _fileUploadService = fileUploadService;
            _mediator = mediator;
            _loggedUserService = loggedUserService;
        }




        [HttpPost("Create")]
        public async Task<ActionResult<Result<Guid>>> Create([FromForm] CreateDocumentCommand command, CancellationToken cancellationToken)
        {
            if (command.Files == null || !command.Files.Any())
            {
                return BadRequest("Dosya eklenmedi.");
            }
            command.CreatedBy = Guid.Parse("11be013a-eba2-4d40-8cb3-12c07e345d69");
            command.TableName = "Deneme";
            var result = await _mediator.Send(command);
            return Ok(result);
        }



        [HttpDelete("{id}")]
        public async Task<ActionResult<Result<Guid>>> Delete(Guid id)
        {
            return await _mediator.Send(new DeleteDocumentsCommand(id));
        }



    }
}