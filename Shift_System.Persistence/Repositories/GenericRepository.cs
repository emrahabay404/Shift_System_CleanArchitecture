using Microsoft.EntityFrameworkCore;
using Shift_System.Application.Interfaces.Repositories;
using Shift_System.Domain.Common;
using Shift_System.Persistence.Contexts;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;

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

        public async Task<List<T>> GetAllAsync(
            Expression<Func<T, bool>>? filter = null,
            string? orderBy = null,
            int? page = null,
            int? pageSize = null)
        {
            IQueryable<T> query = _dbContext.Set<T>();

            // Filtre uygulanacaksa
            if (filter != null)
            {
                query = query.Where(filter);
            }

            // Sıralama uygulanacaksa
            if (!string.IsNullOrEmpty(orderBy))
            {
                query = query.OrderBy(orderBy); // Dynamic LINQ kullanımı
            }

            // Sayfalama uygulanacaksa
            if (page.HasValue && pageSize.HasValue)
            {
                query = query
                    .Skip((page.Value - 1) * pageSize.Value)
                    .Take(pageSize.Value);
            }

            return await query.ToListAsync();
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }
    }
}