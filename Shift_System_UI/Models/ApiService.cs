using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace Shift_System_UI.Models
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _HttpContextAccessor;

        public ApiService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
            _HttpContextAccessor = httpContextAccessor;
        }

        public async Task<List<TeamResponse>> GetTeamsAsync()
        {
            try
            {
                // JWT token'ı al
                var token = _HttpContextAccessor.HttpContext.Session.GetString("JWTToken");

                if (string.IsNullOrEmpty(token))
                {
                    throw new Exception("JWT Token eksik veya geçersiz.");
                }

                // Authorization başlığını dinamik olarak her istekten önce ekle
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                // API'ye GET isteği gönder
                var response = await _httpClient.GetAsync("/api/teams");

                if (response.IsSuccessStatusCode)
                {
                    var jsonResult = await response.Content.ReadAsStringAsync();
                    var model = JsonConvert.DeserializeObject<List<TeamResponse>>(jsonResult);
                    return model;
                }
                else
                {
                    // Hata mesajı al ve özel bir hata oluştur
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    throw new Exception($"API hatası: {response.StatusCode} - {response.ReasonPhrase}. Detay: {errorMessage}");
                }
            }
            catch (HttpRequestException httpEx)
            {
                // HTTP isteği sırasında bir hata oluştuğunda loglama yapın veya özel bir hata fırlatın
                throw new Exception($"HTTP isteği sırasında bir hata oluştu: {httpEx.Message}", httpEx);
            }
            catch (Exception ex)
            {
                // Genel hata yakalama bloğu
                throw new Exception($"Bir hata oluştu: {ex.Message}", ex);
            }
        }

    }
}
