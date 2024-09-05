using Dapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shift_System.Application.Features.Shifts.Queries;
using Shift_System.Application.Interfaces;
using Shift_System.Domain.Entities.Models;
using Shift_System.Infrastructure.Services;
using Shift_System.Persistence.Contexts;
using Shift_System_UI.Models;
using System.Diagnostics;

namespace Shift_System_UI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class HomeController : Controller
    {
        private readonly ApiService _apiService;
        private readonly IHttpClientFactory _httpClientFactory;
        private static string apiTeamsEndpoint = "/api/teams";
        private readonly IMediator _mediator;
        private readonly IFileUploadService _fileUploadService;

        public HomeController(IHttpClientFactory httpClientFactory, ApiService apiService, IMediator mediator, IFileUploadService fileUploadService)
        {
            _httpClientFactory = httpClientFactory;
            _apiService = apiService;
            _mediator = mediator;
            _fileUploadService = fileUploadService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            using var _cnn = DapperContext.CreateConnection();
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
            var teamResponses = _cnn.Query<TeamResponse>(query).ToList();
            var viewModel = new TeamsModel()
            {
                Teams = teamResponses
            };
            return View(viewModel);
        }

        [HttpGet]
        public async Task<JsonResult> GetShiftsWithPagination([FromQuery] GetShiftsWithPaginationQuery query)
        {
            var validator = new GetShiftsWithPaginationValidator();
            var result = validator.Validate(query);

            if (result.IsValid)
            {
                var data = await _mediator.Send(query);
                return new JsonResult(data);
            }

            var errorMessages = result.Errors.Select(x => x.ErrorMessage).ToList();
            return new JsonResult(new { errors = errorMessages }) { StatusCode = 400 };
        }

        // POST: FileUpload/Upload
        [HttpPost]
        public async Task<JsonResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return Json(new { success = false, message = "Dosya seçilmedi." });
            }

            // Yükleme yapılacak klasör
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");

            try
            {
                // Servis ile dosya yükleme işlemini gerçekleştir
                var filePath = await _fileUploadService.UploadFileAsync(file, folderPath);

                if (string.IsNullOrEmpty(filePath))
                {
                    return Json(new { success = false, message = "Dosya yüklenemedi." });
                }

                return Json(new { success = true, message = "Dosya başarıyla yüklendi!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Dosya yükleme hatası: " + ex.Message });
            }
        }

        [HttpPost]
        public IActionResult DeleteFile(string fileName)
        {
            // Uygulamanın kök dizinine 'wwwroot/uploads' klasörünü ekleyelim
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

            // Gelen dosya adını kontrol edelim ve loglayalım
            Console.WriteLine($"Silinmek istenen dosya adı: {fileName}");

            // Dosya uzantısızsa dizindeki tüm dosyalar arasında arama yapalım
            var filesInDirectory = Directory.GetFiles(folderPath);
            string fullFilePath = null;

            // Uzantısız dosya adını bul ve eşleşen dosyayı tespit et
            foreach (var file in filesInDirectory)
            {
                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file);
                if (fileNameWithoutExtension.Equals(fileName, StringComparison.OrdinalIgnoreCase))
                {
                    fullFilePath = file;  // Eşleşen dosya bulundu
                    break;
                }
            }

            // Eşleşen dosya bulunamazsa, dosya mevcut değil
            if (fullFilePath == null)
            {
                Console.WriteLine("Dosya bulunamadı.");
                return Json(new { success = false, message = $"Dosya bulunamadı: {fileName}" });
            }

            // Servis aracılığıyla dosyayı silme işlemini gerçekleştirelim
            var fileNameWithExtension = Path.GetFileName(fullFilePath); // Uzantılı dosya adını alalım
            bool result = _fileUploadService.DeleteFile(folderPath, fileNameWithExtension);

            if (result)
            {
                return Json(new { success = true, message = "Dosya başarıyla silindi!" });
            }

            return Json(new { success = false, message = $"Dosya silinemedi: {fileNameWithExtension}" });
        }

        [HttpGet]
        public async Task<IActionResult> Deneme()
        {
            //return View();
            try
            {
                var teams = await _apiService.GetAsync<List<TeamResponse>>(apiTeamsEndpoint);
                return View(teams); // Doğrudan View'e model olarak gönder
            }
            catch (Exception ex)
            {
                // Hata yönetimi
                var errorViewModel = new ErrorViewModel
                {
                    Url = apiTeamsEndpoint,
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    Message = ex.Message // Hata mesajını modelde saklayın
                };
                return View("Error", errorViewModel);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(ErrorViewModel model)
        {
            return View(new ErrorViewModel { Message = model.Message, RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            //return View(new ErrorViewModel { Message = h,RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
