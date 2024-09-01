using Dapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shift_System.Persistence.Contexts;

namespace Shift_System.WebAPI.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : ApiControllerBase
    {

        private readonly IMediator _mediator;
        private readonly ILogger<AuthController> _logger;

        public TeamsController(IMediator mediator, ILogger<AuthController> logger)
        {
            _mediator = mediator;
            _logger = logger;
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

    }
}