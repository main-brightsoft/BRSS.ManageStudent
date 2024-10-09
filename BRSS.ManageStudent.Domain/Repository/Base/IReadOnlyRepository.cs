using BRSS.ManageStudent.Domain.Entity.Base;

namespace BRSS.ManageStudent.Domain.Repository.Base;

public interface IReadOnlyRepository<TEntity, TKey> where TEntity: IEntity<TKey>
{
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<TEntity> GetAsync(TKey id);
}