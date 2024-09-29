using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shift_System.Domain.Entities;
using Shift_System.Domain.Entities.Models;
using Shift_System.Shared.Helpers;
using System.Text;

namespace Shift_System_UI.Controllers
{
    [AllowAnonymous]
    public class AuthController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthController(SignInManager<AppUser> signInManager, IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager)
        {
            _signInManager = signInManager;
            _httpClient = httpClientFactory.CreateClient("ApiClient");
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        [HttpGet]
        [Route("/Account/Login/")]
        public IActionResult Login(string returnUrl = null)
        {
            // ReturnUrl uzun ve güvensizse, geçerli bir yerel URL değilse bunu kontrol edelim
            if (!string.IsNullOrEmpty(returnUrl) && returnUrl.Length > 2000)
            {
                // Eğer URL aşırı uzun ve güvenli değilse ana sayfaya yönlendirelim
                return RedirectToAction("Index", "Home");
            }
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> Login(string username, string password)
        {
            // Kullanıcı adı ve şifre ile kimlik doğrulama yap
            var result = await _signInManager.PasswordSignInAsync(username, password, true, true);
            if (!result.Succeeded)
            {
                return Json(Messages.Login_Failed_TR);
            }
            try
            {
                return Json(true);
            }
            catch (Exception ex)
            {
                return Json(Messages.Operation_Failed_TR + ex.Message);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> Register(RegistrationModel model)
        {
            // Yeni bir kullanıcı oluşturuyoruz
            var user = new AppUser
            {
                UserName = model.Username,
                FullName = model.Name + model.Surname,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
            };
            // Kullanıcıyı oluşturmaya çalışıyoruz
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                // Eğer başarılıysa, otomatik olarak giriş yapabiliriz
                await _signInManager.SignInAsync(user, isPersistent: false);
                return Json(new { success = true, message = "Kullanıcı başarıyla kaydedildi ve giriş yapıldı." });
            }
            // Eğer hata oluşursa, hata mesajlarını geri döndürüyoruz
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return Json(new { success = false, message = $"Kayıt işlemi başarısız: {errors}" });
        }

        [HttpPost]
        [Authorize]
        public async Task<JsonResult> ApiLogin(string password)
        {
            string username = User.Identity.Name;
            if (string.IsNullOrEmpty(username))
            {
                return Json(new { success = false, message = Messages.Login_Failed_TR });
            }

            // Kimlik doğrulama başarılıysa, API'ye login isteği yap
            var loginModel = new LoginModel(username, password);
            var content = new StringContent(JsonConvert.SerializeObject(loginModel), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(ApiEndPoints.ApiLoginEnddpoint, content);
            if (!response.IsSuccessStatusCode)
            {
                return Json(new { success = false, message = Messages.Login_Failed_TR });
            }

            // Yanıtı JSON olarak deserializ edelim
            var responseContent = await response.Content.ReadAsStringAsync();
            var loginResponse = JsonConvert.DeserializeObject<dynamic>(responseContent);

            // Token'ı al
            string token = loginResponse?.token?.ToString();

            if (string.IsNullOrEmpty(token))
            {
                return Json(new { success = false, message = Messages.Token_Could_Not_Be_Received_TR });
            }

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
                return Json(new { success = false, message = Messages.Jwt_Token_Storage_Failed_TR });
            }

            // Mesaj yoksa varsayılan başarı mesajı gönder
            return Json(new { success = true, message = loginResponse?.message?.ToString() ?? Messages.Token_Created_Success_TR, token = token });
        }

        public JsonResult ApiConnectionStatus()
        {
            var testToken = _httpContextAccessor.HttpContext.Session.GetString("JWTToken");
            if (string.IsNullOrEmpty(testToken))
            {
                return Json(new { success = false, message = "JWT Token saklama başarısız!" });
            }
            return Json(new { success = true });
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
