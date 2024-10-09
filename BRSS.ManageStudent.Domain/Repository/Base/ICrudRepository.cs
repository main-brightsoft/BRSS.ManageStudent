using BRSS.ManageStudent.Domain.Entity.Base;

namespace BRSS.ManageStudent.Domain.Repository.Base;

public interface ICrudRepository<TEntity,TKey>: IReadOnlyRepository<TEntity, TKey> where TEntity: IEntity<TKey>
{
    Task<TEntity> AddAsync(TEntity entity);
    Task<TEntity> UpdateAsync(TEntity entity);
    Task DeleteAsync(TKey id);
    Task DeleteManyAsync(IEnumerable<TKey> ids);
}