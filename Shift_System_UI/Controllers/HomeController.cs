using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        private static string apiTeamsEndpoint = "/api/teams"; // Statik değişken tanımı

        public HomeController(IHttpClientFactory httpClientFactory, ApiService apiService)
        {
            _httpClientFactory = httpClientFactory;
            _apiService = apiService;
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
        public JsonResult IndexMethod(int? pageNumber = 1, int? pageSize = 2)
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
        p.PersonelKodu
    OFFSET @Offset ROWS 
    FETCH NEXT @PageSize ROWS ONLY;
";

            // Varsayılan değerleri kullanarak null kontrolü yapalım
            var currentPageNumber = pageNumber ?? 1;
            var currentPageSize = pageSize ?? 250;

            var offset = (currentPageNumber - 1) * currentPageSize;

            var teamResponses = _cnn.Query<TeamResponse>(query, new { Offset = offset, PageSize = currentPageSize }).ToList();

            var totalCountQuery = "SELECT COUNT(*) FROM Personel"; // Toplam kayıt sayısını almak için basit bir sorgu
            var totalCount = _cnn.ExecuteScalar<int>(totalCountQuery);

            var viewModel = new
            {
                Teams = teamResponses,
                TotalCount = totalCount
            };

            return Json(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Deneme()
        {
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
