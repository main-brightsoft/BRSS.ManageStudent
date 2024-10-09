using BRSS.ManageStudent.Application.Interface.Base;
using BRSS.ManageStudent.Domain.Entity.Base;
using BRSS.ManageStudent.Domain.Repository.Base;

namespace BRSS.ManageStudent.Application.Service.Base;

public abstract class ReadOnlyService <TEntity, TEntityDTO, TKey>: IReadOnlyService<TEntityDTO, TKey> 
    where TEntityDTO : class where TEntity : IEntity<TKey>
{
    protected readonly IReadOnlyRepository<TEntity, TKey> _repository;
    
    protected ReadOnlyService(IReadOnlyRepository<TEntity, TKey> repository)
    {
        _repository = repository;
    }
    
    public async Task<IEnumerable<TEntityDTO>> GetAllAsync()
    {
        var entities =await _repository.GetAllAsync();
        var result = entities.Select(entity => MapEntityToEntityDto(entity));
        return result;
    }

    public async Task<TEntityDTO> GetAsync(TKey id)
    {
        var entity = await _repository.GetAsync(id);
        var result = MapEntityToEntityDto(entity);
        return result;
    }
    protected abstract TEntityDTO MapEntityToEntityDto(TEntity entity);

}