using Dapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shift_System.Persistence.Contexts;
using Shift_System_UI.Models;
using System.Diagnostics;

namespace Shift_System_UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMediator _mediator;
        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger, IMediator mediator, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _mediator = mediator;
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
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
                var teamResponses = _cnn.Query<TeamResponse>(query).ToList();
                var viewModel = new TeamsModel(_httpClientFactory)
                {
                    Teams = teamResponses
                };
                return View(viewModel);
            }
        }



        [HttpGet]
        public JsonResult IndexMethod(int? pageNumber = 1, int? pageSize = 2)
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
        }



        //EĞER APİYE İSTEK ATILACAKSA KULLANILABİLİR.
        //[AllowAnonymous]
        public async Task<IActionResult> Teams()
        {
            // IHttpClientFactory'yi kullanarak TeamsModel'i oluşturun
            TeamsModel tms = new TeamsModel(_httpClientFactory);
            // Model üzerinde veri çekme işlemi gerçekleştir
            await tms.OnGetAsync("");
            return View(tms); // Views/Home/Teams.cshtml dosyasını render eder.
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
