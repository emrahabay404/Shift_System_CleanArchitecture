using Shift_System.Domain.Common.Interfaces;
using System.Linq.Expressions;

namespace Shift_System.Application.Interfaces.Repositories
{
    public interface IGenericRepository<T> where T : class, IEntity
    {
        IQueryable<T> Entities { get; }

        Task<T> GetByIdAsync(Guid id);
        Task<List<T>> GetAllAsync(
            Expression<Func<T, bool>>? filter = null,
            string? orderBy = null,
            int? page = null,
            int? pageSize = null);
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}