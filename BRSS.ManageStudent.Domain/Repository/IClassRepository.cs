using BRSS.ManageStudent.Domain.Entity;
using BRSS.ManageStudent.Domain.Repository.Base;

namespace BRSS.ManageStudent.Domain.Repository;

public interface IClassRepository:ICrudRepository<Class, Guid>
{
    
}