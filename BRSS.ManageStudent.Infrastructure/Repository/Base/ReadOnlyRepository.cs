using BRSS.ManageStudent.Application.UnitOfWork;
using BRSS.ManageStudent.Domain.Entity.Base;
using BRSS.ManageStudent.Domain.Exception;
using BRSS.ManageStudent.Domain.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace BRSS.ManageStudent.Infrastructure.Repository.Base;
public abstract class ReadOnlyRepository<TEntity, TKey> : IReadOnlyRepository<TEntity, TKey>  where TEntity : class, IEntity<TKey>
{
    private readonly IUnitOfWork _unitOfWork;

    protected ReadOnlyRepository(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await _unitOfWork.Context.Set<TEntity>().ToListAsync();
    }

    public virtual async Task<TEntity> GetAsync(TKey id)
    {
        var result = await _unitOfWork.Context.Set<TEntity>().FindAsync(id);
        if (result == null)
        {
            throw new NotFoundException($"Entity of type {typeof(TEntity).Name} with ID {id} was not found.");
        }
        return result;
    }
    
}