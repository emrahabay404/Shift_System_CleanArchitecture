using Dapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shift_System.Application.Features.Shifts.Queries;
using Shift_System.Domain.Entities.Models;
using Shift_System.Infrastructure.Services;
using Shift_System.Persistence.Contexts;
using Shift_System.Shared.Helpers;
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


        public HomeController(IHttpClientFactory httpClientFactory, ApiService apiService, IMediator mediator)
        {
            _httpClientFactory = httpClientFactory;
            _apiService = apiService;
            _mediator = mediator;
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


        [HttpGet]
        public async Task<IActionResult> Deneme()
        {
            return View();
            //try
            //{
            //    var teams = await _apiService.GetAsync<List<TeamResponse>>(apiTeamsEndpoint);
            //    return View(teams); // Doğrudan View'e model olarak gönder
            //}
            //catch (Exception ex)
            //{
            //    // Hata yönetimi
            //    var errorViewModel = new ErrorViewModel
            //    {
            //        Url = apiTeamsEndpoint,
            //        RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
            //        Message = ex.Message // Hata mesajını modelde saklayın
            //    };
            //    return View("Error", errorViewModel);
            //}
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(ErrorViewModel model)
        {
            return View(new ErrorViewModel { Message = model.Message, RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            //return View(new ErrorViewModel { Message = h,RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
