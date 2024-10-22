using Microsoft.EntityFrameworkCore;
using Shift_System.Application.Interfaces.Repositories;
using Shift_System.Domain.Common;
using Shift_System.Persistence.Contexts;
using Shift_System.Shared.Helpers;
using System.Linq.Expressions;
using System.Text.Json;

namespace Shift_System.Persistence.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseAuditableEntity
    {
        private readonly ApplicationDbContext _dbContext;

        public GenericRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<T> Entities => _dbContext.Set<T>();

        public async Task<T> AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            return entity;
        }

        public Task UpdateAsync(T entity)
        {
            T exist = _dbContext.Set<T>().Find(entity.Id);
            _dbContext.Entry(exist).CurrentValues.SetValues(entity);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            return Task.CompletedTask;
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _dbContext
                .Set<T>()
                .ToListAsync();
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }




        public IQueryable<T> ApplyDynamicQuery(DynamicQuery query)
        {
            var dbQuery = Entities.AsQueryable();

            // Eğer filtre varsa uygula
            if (query.Filter != null)
            {
                dbQuery = ApplyFilters(dbQuery, query.Filter);
            }

            // Sıralama uygula
            if (query.Sort != null)
            {
                dbQuery = ApplySorting(dbQuery, query.Sort);
            }

            // Sayfalama işlemi
            dbQuery = dbQuery
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize);

            return dbQuery;
        }

        private IQueryable<T> ApplyFilters(IQueryable<T> query, Filter filter)
        {
            if (filter.Filters != null && filter.Filters.Any())
            {
                var parameter = Expression.Parameter(typeof(T), "x");
                Expression? combined = null;

                foreach (var subFilter in filter.Filters)
                {
                    var member = Expression.Property(parameter, subFilter.Field);
                    var targetType = Nullable.GetUnderlyingType(member.Type) ?? member.Type;
                    var convertedValue = ConvertValue(subFilter.Value, targetType);
                    var constant = Expression.Constant(convertedValue, member.Type);

                    Expression? body = subFilter.Operator.ToLower() switch
                    {
                        "eq" => Expression.Equal(member, constant),
                        "contains" when member.Type == typeof(string) =>
                            Expression.Call(member, typeof(string).GetMethod("Contains", new[] { typeof(string) })!, constant),
                        _ => throw new NotSupportedException($"Operator {subFilter.Operator} not supported.")
                    };

                    combined = combined == null
                        ? body
                        : filter.Logic?.ToLower() == "or"
                            ? Expression.OrElse(combined, body)
                            : Expression.AndAlso(combined, body);
                }

                var predicate = Expression.Lambda<Func<T, bool>>(combined!, parameter);
                query = query.Where(predicate);
            }
            return query;
        }

        private IQueryable<T> ApplySorting(IQueryable<T> query, IEnumerable<Sort> sorts)
        {
            bool isFirst = true;

            foreach (var sort in sorts)
            {
                var parameter = Expression.Parameter(typeof(T), "x");
                var member = Expression.Property(parameter, sort.Field);
                var keySelector = Expression.Lambda(member, parameter);

                if (isFirst)
                {
                    query = sort.Dir.ToLower() == "asc"
                        ? Queryable.OrderBy(query, (dynamic)keySelector)
                        : Queryable.OrderByDescending(query, (dynamic)keySelector);
                    isFirst = false;
                }
                else
                {
                    query = sort.Dir.ToLower() == "asc"
                        ? Queryable.ThenBy((IOrderedQueryable<T>)query, (dynamic)keySelector)
                        : Queryable.ThenByDescending((IOrderedQueryable<T>)query, (dynamic)keySelector);
                }
            }

            return query;
        }

        private object ConvertValue(object value, Type targetType)
        {
            if (value is JsonElement jsonElement)
            {
                if (targetType == typeof(bool))
                    return jsonElement.GetBoolean();
                if (targetType == typeof(int))
                    return jsonElement.GetInt32();
                if (targetType == typeof(string))
                    return jsonElement.GetString();
            }

            return Convert.ChangeType(value, targetType);
        }


    }
}
