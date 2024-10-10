using Dapper;
using Microsoft.AspNetCore.Mvc;
using Shift_System.Persistence.Contexts;
using System.Security.Claims;

namespace Shift_System.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ApiControllerBase
    {
        [HttpGet]
        [Route("TwoFactorEnabled")]
        public IActionResult TwoFactorEnabled()
        {
            // JWT'den kullanıcı ID'sini alıyoruz
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized("Kullanıcı doğrulanamadı.");
            }

            using (var _cnn = DapperContext.CreateConnection())
            {
                string query = @"Update AspNetUsers set TwoFactorEnabled = 1 Where Id = @UserId"; // 0 yaparak kapatıyoruz

                // Parametreyi ekliyoruz ve sorguyu çalıştırıyoruz
                var parameters = new { UserId = userId };
                var affectedRows = _cnn.Execute(query, parameters); // Dapper Execute metodu ile çalıştırıyoruz

                if (affectedRows > 0)
                {
                    return Ok(new { success = true, message = "İki faktörlü doğrulama etkinleştirildi" });
                }
                else
                {
                    return BadRequest("Güncelleme başarısız.");
                }
            }
        }

        [HttpGet]
        [Route("TwoFactorDisabled")]
        public IActionResult TwoFactorDisabled()
        {
            // JWT'den kullanıcı ID'sini alıyoruz
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized("Kullanıcı doğrulanamadı.");
            }

            using (var _cnn = DapperContext.CreateConnection())
            {
                string query = @"Update AspNetUsers set TwoFactorEnabled = 0 Where Id = @UserId"; // 0 yaparak kapatıyoruz

                // Parametreyi ekliyoruz ve sorguyu çalıştırıyoruz
                var parameters = new { UserId = userId };
                var affectedRows = _cnn.Execute(query, parameters); // Dapper Execute metodu ile çalıştırıyoruz

                if (affectedRows > 0)
                {
                    return Ok(new { success = true, message = "İki faktörlü doğrulama kapatıldı." });
                }
                else
                {
                    return BadRequest("Güncelleme başarısız.");
                }
            }
        }

        [HttpGet]
        [Route("GetMyUserID")]
        public IActionResult GetMyUserID()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Ok(userId);
        }

        [HttpGet]
        [Route("GetMyUserName")]
        public IActionResult GetMyUserName()
        {
            var username = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
            return Ok(username);
        }

        [HttpGet]
        [Route("GetMyEmail")]
        public IActionResult GetMyEmail()
        {
            var username = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
            return Ok(username);
        }

        [HttpGet]
        [Route("GetMyFullName")]
        public IActionResult GetMyFullName()
        {
            var username = HttpContext.User.FindFirst(ClaimTypes.GivenName)?.Value;
            return Ok(username);
        }
    }
}
