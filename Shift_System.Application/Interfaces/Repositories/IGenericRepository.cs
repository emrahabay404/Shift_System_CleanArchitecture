using Shift_System.Domain.Common.Interfaces;
using Shift_System.Shared.Helpers;

namespace Shift_System.Application.Interfaces.Repositories
{
    public interface IGenericRepository<T> where T : class, IEntity
    {
        IQueryable<T> Entities { get; }
        Task<T> GetByIdAsync(Guid id);
        Task<List<T>> GetAllAsync();
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);



        // Dinamik sorgu metodu ekliyoruz
        IQueryable<T> ApplyDynamicQuery(DynamicQuery query);
    }
}
