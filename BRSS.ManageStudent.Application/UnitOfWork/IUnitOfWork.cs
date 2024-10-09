using Microsoft.EntityFrameworkCore;

namespace BRSS.ManageStudent.Application.UnitOfWork;
public interface IUnitOfWork : IDisposable, IAsyncDisposable
{
    DbContext Context { get; }
    public Task<int> SaveChangesAsync();
    void BeginTransaction();
    Task BeginTransactionAsync();
    void Commit();
    Task CommitAsync();
    void Rollback();
    Task RollbackAsync();
}
