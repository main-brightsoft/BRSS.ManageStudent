using BRSS.ManageStudent.Application.Interface.Base;
using BRSS.ManageStudent.Domain.Entity.Base;
using BRSS.ManageStudent.Domain.Repository.Base;

namespace BRSS.ManageStudent.Application.Service.Base;

public abstract class CrudService<TEntityDTO, TEntityCreateDTO, TEntityUpdateDTO, TEntity, TKey>: ReadOnlyService<TEntity, TEntityDTO, TKey>, ICrudService<TEntityDTO, TEntityCreateDTO, TEntityUpdateDTO,TKey> 
    where TEntityDTO : class where TEntityCreateDTO : class where TEntityUpdateDTO : class where TEntity : IEntity<TKey>
{
    protected readonly ICrudRepository<TEntity, TKey> _repository;
    
    protected CrudService(ICrudRepository<TEntity, TKey> repository) : base(repository)
    {
        _repository = repository;
    }

    public async Task<TEntityDTO> AddAsync(TEntityCreateDTO entityCreateDto)
    {
        var entity = await MapEntityCreateDtoToEntity(entityCreateDto);
        await _repository.AddAsync(entity);
        var result = MapEntityToEntityDto(entity);
        return result;
    }

    public async Task<TEntityDTO> UpdateAsync(TKey id, TEntityUpdateDTO entityUpdateDto)
    {
        var entity = await MapEntityUpdateDtoToEntity(id, entityUpdateDto);
        entity.SetId(id);
        await _repository.UpdateAsync(entity);
        var result = MapEntityToEntityDto(entity);
        return result;
    }

    public async Task DeleteAsync(TKey id)
    {
        await _repository.DeleteAsync(id);
    }

    public async Task DeleteManyAsync(List<TKey> ids)
    {
        await _repository.DeleteManyAsync(ids);
    }
    
    protected abstract Task<TEntity> MapEntityCreateDtoToEntity(TEntityCreateDTO entityCreateDTO);
    protected abstract Task<TEntity> MapEntityUpdateDtoToEntity(TKey id, TEntityUpdateDTO entityUpdateDTO);
}
