using BRSS.ManageStudent.Application.UnitOfWork;
using BRSS.ManageStudent.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace BRSS.ManageStudent.Infrastructure.UnitOfWork;

 public sealed class UnitOfWork(ApplicationDbContext context) : IUnitOfWork
 {
    private IDbContextTransaction? _transaction;

    public DbContext Context { get; } = context;
    
    public void BeginTransaction()
    {
        _transaction ??= Context.Database.BeginTransaction();
    }

    public async Task BeginTransactionAsync()
    {
        _transaction ??= await Context.Database.BeginTransactionAsync();
    }

    public void Commit()
    {
        if (_transaction != null)
        {
            _transaction.Commit();
            _transaction.Dispose();
            _transaction = null;
        }
    }

    public async Task CommitAsync()
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Rollback()
    {
        if (_transaction != null)
        {
            _transaction.Rollback();
            _transaction.Dispose();
            _transaction = null;
        }
    }

    public async Task RollbackAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        Context.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        if (_transaction != null)
        {
            await _transaction.DisposeAsync();
        }
        await Context.DisposeAsync();
    }
}