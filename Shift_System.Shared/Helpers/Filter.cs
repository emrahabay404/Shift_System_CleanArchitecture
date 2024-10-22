namespace Shift_System.Shared.Helpers
{
    public class Filter
    {
        public string Field { get; set; } // Filtrelenecek alan adı
        public object? Value { get; set; } // Dinamik veri tipi desteği
        public string Operator { get; set; } // Operatör: eq, contains, vb.
        public string? Logic { get; set; } = "and"; // Mantıksal Operatör: "and" veya "or"
        public IEnumerable<Filter>? Filters { get; set; } // Alt filtreler desteği

        public Filter()
        {
            Field = string.Empty;
            Operator = string.Empty;
        }

        public Filter(string field, string @operator)
        {
            Field = field;
            Operator = @operator;
        }
    }
}
