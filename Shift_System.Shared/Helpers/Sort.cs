namespace Shift_System.Shared.Helpers
{
    public class Sort
    {
        public string Field { get; set; } // Sıralanacak alan adı
        public string Dir { get; set; } // Sıralama yönü: "asc" veya "desc"

        public Sort()
        {
            Field = string.Empty;
            Dir = string.Empty;
        }

        public Sort(string field, string dir)
        {
            Field = field;
            Dir = dir;
        }
    }
}
