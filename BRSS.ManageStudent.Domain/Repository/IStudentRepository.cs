using BRSS.ManageStudent.Domain.Entity;
using BRSS.ManageStudent.Domain.Repository.Base;

namespace BRSS.ManageStudent.Domain.Repository;

public interface IStudentRepository : ICrudRepository<Student, Guid>
{
    Task<List<Student>> GetByIdsAsync(IEnumerable<Guid> ids);
}