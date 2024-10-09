using BRSS.ManageStudent.Application.UnitOfWork;
using BRSS.ManageStudent.Domain.Entity;
using BRSS.ManageStudent.Domain.Exception;
using BRSS.ManageStudent.Domain.Repository;
using BRSS.ManageStudent.Infrastructure.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace BRSS.ManageStudent.Infrastructure.Repository;

public class StudentRepository(IUnitOfWork unitOfWork) : CrudRepository<Student, Guid>(unitOfWork), IStudentRepository
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<List<Student>> GetByIdsAsync(IEnumerable<Guid> ids)
    {
        var studentList = await _unitOfWork.Context.Set<Student>()
            .Where(s => ids.Contains(s.Id))
            .ToListAsync();

        var missingIds = ids.Except(studentList.Select(s => s.Id)).ToList();
        if (missingIds.Count != 0)
        {
            throw new NotFoundException($"Students with the following IDs were not found: {string.Join(", ", missingIds)}");
        }

        return studentList;
    }

    public override async Task DeleteAsync(Guid id)
    {
        var entity = await _unitOfWork.Context.Set<Student>()
            .Include(s => s.Classes)
            .FirstOrDefaultAsync(s => s.Id == id);
        if (entity == null) 
        {
            throw new Exception($"Entity of type {nameof(Student)} with ID {id} was not found for deletion.");
        }
        entity.Classes.Clear();
        _unitOfWork.Context.Set<Student>().Remove(entity);
        var result = await _unitOfWork.Context.SaveChangesAsync();
            
        if (result == 0) 
        {
            throw new Exception($"Failed to delete entity of type {nameof(Student)}.");
        }
    }
}