using Dapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shift_System.Application.Features.Shifts.Queries;
using Shift_System.Persistence.Contexts;
using Shift_System.Shared.Helpers;

namespace Shift_System.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class TeamsController : ApiControllerBase
    {
        private readonly IMediator _mediator;
        public TeamsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public ActionResult Index()
        {
            using (var _cnn = DapperContext.CreateConnection())
            {
                string query = @"
        SELECT 
            p.PersonelKodu,
            p.Adi AS PersonelAdi,
            p.Soyadi AS PersonelSoyadi,
            t.TakimAdi,
            v.VardiyaAdi,
            v.BaslangicTarihi,
            v.BitisTarihi
        FROM 
            Personel p
        JOIN 
            TakimPersonel tp ON p.PersonelID = tp.PersonelID
        JOIN 
            Takim t ON tp.TakimID = t.TakimID
        JOIN 
            VardiyaTakim vt ON t.TakimID = vt.TakimID
        JOIN 
            Vardiya v ON vt.VardiyaID = v.VardiyaID
        ORDER BY 
            p.PersonelKodu;
        ";
                var teamResponses = _cnn.Query(query).ToList();

                return Ok(teamResponses);
            }
        }


        [HttpGet("paged")]
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

    }
}