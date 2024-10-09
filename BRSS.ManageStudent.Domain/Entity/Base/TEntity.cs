
namespace BRSS.ManageStudent.Domain.Entity.Base;

public interface IEntity<TKey>
{
    TKey GetId();
    void SetId(TKey id);
}