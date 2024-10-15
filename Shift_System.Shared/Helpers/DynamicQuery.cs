namespace Shift_System.Shared.Helpers
{
    public class DynamicQuery
    {
        public int Page { get; set; } = 1; // Varsayılan olarak 1. sayfa
        public int PageSize { get; set; } = 250; // Varsayılan olarak 10 kayıt
        public IEnumerable<Sort>? Sort { get; set; }
        public Filter? Filter { get; set; }

        public DynamicQuery()
        {

        }

        public DynamicQuery(IEnumerable<Sort>? sort, Filter? filter)
        {
            Filter = filter;
            Sort = sort;
        }
    }
}
