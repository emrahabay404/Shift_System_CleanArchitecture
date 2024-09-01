using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shift_System.Domain.Entities;
using Shift_System.Domain.Entities.Models;
using System.Text;

namespace Shift_System_UI.Controllers
{
    [AllowAnonymous]
    public class AuthController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AuthController(SignInManager<AppUser> signInManager, IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _signInManager = signInManager;
            _httpClient = httpClientFactory.CreateClient("ApiClient");
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        [Route("/Account/Login/")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> Login(string username, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(username, password, true, true);
            if (result.Succeeded)
            {
                var _model = new LoginModel(username, password);

                var content = new StringContent(JsonConvert.SerializeObject(_model), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("/api/auth/login", content);

                if (response.IsSuccessStatusCode)
                {
                    // Yanıtın düz metin JWT token olduğunu varsayarak doğrudan oku
                    var token = await response.Content.ReadAsStringAsync();

                    // Token'ın başında veya sonunda fazladan karakterler olup olmadığını kontrol edin ve temizleyin
                    token = token.Trim(new char[] { '\"' }); // Eğer token çift tırnak içinde dönerse tırnakları temizler

                    // Token'ı session'a kaydet
                    _httpContextAccessor.HttpContext.Session.SetString("JWTToken", token);

                    // Token'ı cookie'ye raw formatta kaydet
                    _httpContextAccessor.HttpContext.Response.Cookies.Append("JWTToken", token, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict
                    });

                    // Token'ın başarıyla kaydedilip kaydedilmediğini kontrol et
                    var testToken = _httpContextAccessor.HttpContext.Session.GetString("JWTToken");
                    if (string.IsNullOrEmpty(testToken))
                    {
                        throw new Exception("JWT Token saklama başarısız!");
                    }

                    return Json(true);
                }
                else
                {
                    return Json(new { success = false, message = "Login API çağrısı başarısız." });
                }
            }
            else
            {
                return Json(new { success = false, message = "Kullanıcı adı veya şifre hatalı." });
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            // Kullanıcının kimlik doğrulamasını sonlandır
            await _signInManager.SignOutAsync();
            // Tüm çerezleri temizle
            foreach (var cookie in Request.Cookies.Keys)
            {
                Response.Cookies.Delete(cookie);
            }
            // Oturumu (session) temizle
            HttpContext.Session.Clear();
            return Json(true);
        }

        [AllowAnonymous]
        [Route("/Auth/AccessDenied")]
        public IActionResult PageDenied()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Page404()
        {
            return View();
        }
    }
}
