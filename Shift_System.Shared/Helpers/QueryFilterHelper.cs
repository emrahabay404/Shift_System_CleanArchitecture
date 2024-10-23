using Microsoft.EntityFrameworkCore;

namespace Shift_System.Shared.Helpers
{
    public static class QueryFilterHelper
    {
        public static IQueryable<T> ApplyFilter<T>(IQueryable<T> query, Filter filter) where T : class
        {
            // Her zaman IsDeleted = false kontrolü varsa
            if (typeof(T).GetProperty("IsDeleted") != null)
            {
                query = query.Where(e => EF.Property<bool>(e, "IsDeleted") == false);
            }

            // Ana filtreyi uygula
            return ProcessFilter(query, filter);
        }

        private static IQueryable<T> ProcessFilter<T>(IQueryable<T> query, Filter filter) where T : class
        {
            if (filter.Logic == "and")
            {
                query = query.Where(e => EF.Property<object>(e, filter.Field).ToString() == filter.Value.ToString());
            }
            else if (filter.Logic == "or")
            {
                // "or" mantığını burada implement edebilirsiniz
            }

            // Alt filtreleri kontrol et ve uygula
            if (filter.Filters != null && filter.Filters.Any())
            {
                foreach (var subFilter in filter.Filters)
                {
                    query = ProcessFilter(query, subFilter);
                }
            }

            return query;
        }
    }
}