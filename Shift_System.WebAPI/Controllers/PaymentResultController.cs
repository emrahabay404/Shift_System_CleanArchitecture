using Microsoft.AspNetCore.Mvc;
using Shift_System.Domain.Entities.Models;
using System.Security.Cryptography;
using System.Text;

namespace Shift_System.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentResultController : ApiControllerBase
    {
        private readonly IConfiguration _configuration;
        public PaymentResultController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        [HttpPost]
        public IActionResult ReceiveNotification([FromForm] PaymentNotificationModel model)
        {
            string merchant_key = _configuration["PayTR:MerchantKey"];
            string merchant_salt = _configuration["PayTR:MerchantSalt"];
            string hash = model.hash;
            string hash_string = model.merchant_oid + model.status + model.total_amount + merchant_salt + model.hash;

            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(merchant_key)))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(hash_string));
                var computedHashString = Convert.ToBase64String(computedHash);
                if (computedHashString != hash)
                {
                    return BadRequest("Invalid hash");
                }
            }
            if (model.status == "success")
            {
            }
            return Ok("OK");
        }
    }
}
