namespace Shift_System_UI.Models
{
    public class TeamsModel
    {
        public TeamsModel()
        {
            Teams = new List<TeamResponse>();
        }
        public List<TeamResponse> Teams { get; set; }
    }

    public class TeamResponse
    {
        public string PersonelKodu { get; set; } = string.Empty;
        public string PersonelAdi { get; set; } = string.Empty;
        public string PersonelSoyadi { get; set; } = string.Empty;
        public string TakimAdi { get; set; } = string.Empty;
        public string VardiyaAdi { get; set; } = string.Empty;
        public DateTime BaslangicTarihi { get; set; }
        public DateTime BitisTarihi { get; set; }
    }
}
