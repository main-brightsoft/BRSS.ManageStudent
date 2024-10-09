using BRSS.ManageStudent.Domain.Entity.Base;

namespace BRSS.ManageStudent.Application.Interface.Base;

public interface ICrudService<TEntityDTO, TEntityCreateDTO, TEntityUpdateDTO,TKey>: IReadOnlyService<TEntityDTO, TKey> 
    where TEntityDTO : class where TEntityCreateDTO : class where TEntityUpdateDTO: class 
{
    Task<TEntityDTO> AddAsync(TEntityCreateDTO entity);
    Task<TEntityDTO> UpdateAsync(TKey id, TEntityUpdateDTO entity);
    Task DeleteAsync(TKey id);
    Task DeleteManyAsync(IEnumerable<TKey> ids);
}