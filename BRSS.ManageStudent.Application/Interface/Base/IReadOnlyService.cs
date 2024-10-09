namespace BRSS.ManageStudent.Application.Interface.Base;

public interface IReadOnlyService<TEntityDTO, TKey> 
    where TEntityDTO : class
{
    Task<IEnumerable<TEntityDTO>> GetAllAsync();
    Task<TEntityDTO> GetAsync(TKey id);
}