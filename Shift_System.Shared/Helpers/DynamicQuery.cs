namespace Shift_System.Shared.Helpers
{
    public class DynamicQuery
    {
        public List<Filter> Filter { get; set; } = new(); // Çoklu filtreler için liste
        public List<Sort> Sort { get; set; } = new();
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 100;
    }

    public class Filter
    {
        public string Field { get; set; } = string.Empty;
        public string Operator { get; set; } = "eq"; // eq, contains, gt, lt vb.
        public object Value { get; set; }
    }

    public class Sort
    {
        public string Field { get; set; } = string.Empty;
        public string Dir { get; set; } = "asc"; // asc veya desc
    }
}