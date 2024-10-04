using Shift_System.Domain.Common;

namespace Shift_System.Application.Interfaces.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<T> Repository<T>() where T : BaseAuditableEntity;
        Task<int> Save(CancellationToken cancellationToken);
        Task<Guid> SaveAndRemoveCache(CancellationToken cancellationToken, params string[] cacheKeys);
        Task Rollback();
    }
}
