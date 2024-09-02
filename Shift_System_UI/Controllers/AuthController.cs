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
            // Kullanıcı adı ve şifre ile kimlik doğrulama yap
            var result = await _signInManager.PasswordSignInAsync(username, password, true, true);

            if (!result.Succeeded)
            {
                return Json("Kullanıcı adı veya şifre hatalı.");
                //return Json(new { success = false, message = "Kullanıcı adı veya şifre hatalı." });
            }

            // Kimlik doğrulama başarılıysa, API'ye login isteği yap
            var loginModel = new LoginModel(username, password);
            var content = new StringContent(JsonConvert.SerializeObject(loginModel), Encoding.UTF8, "application/json");

            try
            {

                var response = await _httpClient.PostAsync("/api/auth/login", content);

                if (!response.IsSuccessStatusCode)
                {
                    return Json("Login API çağrısı başarısız.");
                    //return Json(new { success = false, message = "Login API çağrısı başarısız." });
                }

                // Yanıtın düz metin JWT token olduğunu varsayarak doğrudan oku
                var token = await response.Content.ReadAsStringAsync();

                // Token'ı temizle (eğer çift tırnak içinde dönüyorsa)
                token = token.Trim(new char[] { '\"' });

                // Token'ı session'a kaydet
                _httpContextAccessor.HttpContext.Session.SetString("JWTToken", token);

                // Token'ı cookie'ye kaydet
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
                    return Json("JWT Token saklama başarısız!");
                    //throw new Exception("JWT Token saklama başarısız!");
                }
                return Json(true);
            }
            catch (Exception ex)
            {
                return Json($"Bir hata oluştu: {ex.Message}");
                //return Json(new { success = false, message = $"Bir hata oluştu: {ex.Message}" });
            }
        }


        [HttpGet]
        [Authorize]
        public IActionResult Logout()
        {
            // Kullanıcıyı sistemden çıkart
            var result = _signInManager.SignOutAsync();
            if (result.IsCompleted)
            {
                // Oturumu temizle
                _httpContextAccessor.HttpContext.Session.Clear();
                // Tüm çerezleri temizle
                if (_httpContextAccessor.HttpContext.Request.Cookies.Count > 0)
                {
                    var cookies = _httpContextAccessor.HttpContext.Request.Cookies.Keys;
                    foreach (var cookie in cookies)
                    {
                        _httpContextAccessor.HttpContext.Response.Cookies.Delete(cookie);
                    }
                }
                return Json(true);
            }
            return Json(false);
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
