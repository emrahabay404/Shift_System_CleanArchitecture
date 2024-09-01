using System.Net.Http.Headers;

namespace Shift_System_UI.Models // Doğru namespace olduğundan emin olun
{
    public class TeamsModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private string _apiBaseUrl = "";
        public TeamsModel(IHttpClientFactory httpClientFactory)
        {
            Teams = new List<TeamResponse>(); // Yanıt türüne uygun listeyi başlatıyoruz.
            _httpClientFactory = httpClientFactory;
        }
        public List<TeamResponse> Teams { get; set; } // API yanıtına uygun yeni sınıf listesi
        public string ErrorMessage { get; set; }

        public async Task OnGetAsync(string token)
        {
            var httpClient = _httpClientFactory.CreateClient();

            try
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                // API'den takımlar listesini çek
                Teams = await httpClient.GetFromJsonAsync<List<TeamResponse>>(_apiBaseUrl + "Teams");
            }
            catch (HttpRequestException httpEx)
            {
                ErrorMessage = $"Error fetching data from the API: {httpEx.Message}";
            }
            catch (Exception ex)
            {
                ErrorMessage = $"An unexpected error occurred: {ex.Message}";
            }
        }
    }

    public class TeamResponse
    {
        public string PersonelKodu { get; set; }
        public string PersonelAdi { get; set; }
        public string PersonelSoyadi { get; set; }
        public string TakimAdi { get; set; }
        public string VardiyaAdi { get; set; }
        public DateTime BaslangicTarihi { get; set; }
        public DateTime BitisTarihi { get; set; }
    }
}
