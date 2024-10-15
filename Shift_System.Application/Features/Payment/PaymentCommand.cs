using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System.Collections.Specialized;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace Application.Features.Payment;

public class PaymentCommand : IRequest<PayResponse>
{
    public Guid? UserId { get; set; }
    public Guid DataId { get; set; }
    public int Price { get; set; }

    public class PaymentCommandHandler : IRequestHandler<PaymentCommand, PayResponse>
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PaymentCommandHandler(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        public async Task<PayResponse> Handle(PaymentCommand request, CancellationToken cancellationToken)
        {
            int price = request.Price * 100; // Kuruşa çevriliyor
            string merchant_id = _configuration["PaymentConfig:MerchantId"];
            string merchant_key = _configuration["PaymentConfig:MerchantKey"];
            string merchant_salt = _configuration["PaymentConfig:MerchantSalt"];
            string emailstr = _configuration["PaymentConfig:Email"];
            string merchant_oid = GenerateUniqueOrderNumber();
            string merchant_ok_url = "http://yourdomain.com/PaymentSuccess";
            string merchant_fail_url = "http://yourdomain.com/PaymentError";
            string user_ip = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? "127.0.0.1";

            object[][] user_basket = {
                new object[] {"Service Description", request.Price.ToString(), 1},
            };

            string user_basket_json = System.Text.Json.JsonSerializer.Serialize(user_basket);
            string user_basketstr = Convert.ToBase64String(Encoding.UTF8.GetBytes(user_basket_json));

            NameValueCollection data = new NameValueCollection
            {
                ["merchant_id"] = merchant_id,
                ["user_ip"] = user_ip,
                ["merchant_oid"] = merchant_oid,
                ["email"] = emailstr,
                ["payment_amount"] = price.ToString(),
                ["user_basket"] = user_basketstr,
                ["debug_on"] = "1",
                ["test_mode"] = "1",
                ["no_installment"] = "1",
                ["max_installment"] = "0",
                ["user_name"] = "Test User",
                ["user_address"] = "Test Address",
                ["user_phone"] = "1234567890",
                ["merchant_ok_url"] = merchant_ok_url,
                ["merchant_fail_url"] = merchant_fail_url,
                ["timeout_limit"] = "10",
                ["currency"] = "TL",
                ["lang"] = "tr"
            };

            string _compare = string.Concat(merchant_id, user_ip, merchant_oid, emailstr, price.ToString(), user_basketstr, "1", "0", "TL", "1", merchant_salt);
            HMACSHA256 hmac = new(Encoding.UTF8.GetBytes(merchant_key));
            byte[] tokenBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(_compare));
            data["Payment_token"] = Convert.ToBase64String(tokenBytes);

            using (WebClient client = new())
            {
                client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                byte[] result = await client.UploadValuesTaskAsync("https://www.payment.com/odeme/api/get-token", "POST", data);
                string resultString = Encoding.UTF8.GetString(result);
                dynamic json = JValue.Parse(resultString);
                if (json.status == "success")
                {
                    string _iframeSrc = "https://www.payment.com/odeme/guvenli/" + json.token;
                    return new PayResponse
                    {
                        IframeSrc = _iframeSrc
                    };
                }
                else
                {
                    throw new Exception("Payment IFRAME failed. reason: " + json.reason);
                }
            }
        }

        private string GenerateUniqueOrderNumber()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random random = new Random();
            return new string(Enumerable.Repeat(chars, 20)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}

public class PayResponse
{
    public string IframeSrc { get; set; } = string.Empty;
}
