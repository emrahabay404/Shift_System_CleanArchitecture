using Dapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shift_System.Application.Features.Shifts.Queries;
using Shift_System.Application.Features.Teams.Commands;
using Shift_System.Application.Interfaces;
using Shift_System.Persistence.Contexts;
using Shift_System.Shared.Helpers;

namespace Shift_System.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : ApiControllerBase
    {
        private readonly IMediator _mediator; private readonly IFileUploadService _fileUploadService;
        public TeamsController(IMediator mediator, IFileUploadService fileUploadService)
        {
            _mediator = mediator;
            _fileUploadService = fileUploadService;
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

        [AllowAnonymous]
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

        [HttpPost("upload-and-create-team")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadAndCreateTeam(IFormFile file, [FromForm] CreateTeamCommand command)
        {
            const long maxFileSize = 5 * 1024 * 1024; // Maksimum dosya boyutu 5 MB

            // Dosya var mı kontrolü
            if (file == null || file.Length == 0)
            {
                return BadRequest(new { success = false, message = "Dosya seçilmedi." });
            }

            // Dosya boyutu kontrolü
            if (file.Length > maxFileSize)
            {
                return BadRequest(new { success = false, message = $"Dosya boyutu {maxFileSize / (1024 * 1024)} MB'ı aşıyor." });
            }

            try
            {
                // Dosya yükleme dizinini ayarlama
                var currentDirectory = Directory.GetCurrentDirectory();
                var folderPathAfterWwwroot = "Uploads";
                var uiProjectDirectory = currentDirectory.Replace(
                    "Shift_System.WebAPI",
                    $@"Shift_System_UI\wwwroot\{folderPathAfterWwwroot}\"
                );

                // Dizin yoksa oluştur
                if (!Directory.Exists(uiProjectDirectory))
                {
                    Directory.CreateDirectory(uiProjectDirectory);
                }

                // Dosya adı oluşturma (benzersiz bir GUID ile)
                var newFileName = $"{Guid.NewGuid()}_{file.FileName}";
                var filePath = Path.Combine(uiProjectDirectory, newFileName);

                // Dosyayı belirtilen yola kaydetme
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Dosya adını CreateTeamCommand'e atayın
                command.FileName = newFileName; // Dosya adı model içinde otomatik olarak atanacak ve kaydedilecek

                // Takım oluşturma işlemi
                var result = await _mediator.Send(command);

                if (result.Succeeded)
                {
                    // Hem dosya hem de takım başarıyla oluşturuldu
                    return Ok(new
                    {
                        success = true,
                        message = "Takım başarıyla oluşturuldu ve dosya başarıyla yüklendi.",
                        filePath,
                        teamResult = result
                    });
                }
                else
                {
                    // Dosya başarıyla yüklendi ama takım oluşturulamadı
                    return BadRequest(new
                    {
                        success = false,
                        message = "Dosya yüklendi ancak takım oluşturulamadı.",
                        filePath
                    });
                }
            }
            catch (Exception ex)
            {
                // Dosya yüklenirken veya takım oluşturulurken bir hata meydana geldi
                return StatusCode(500, new { success = false, message = $"Dosya veya takım işlemleri sırasında hata oluştu: {ex.Message}" });
            }
        }

    }
}