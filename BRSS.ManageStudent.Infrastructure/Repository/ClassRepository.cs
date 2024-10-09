using BRSS.ManageStudent.Application.UnitOfWork;
using BRSS.ManageStudent.Domain.Entity;
using BRSS.ManageStudent.Domain.Exception;
using BRSS.ManageStudent.Domain.Repository;
using BRSS.ManageStudent.Infrastructure.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace BRSS.ManageStudent.Infrastructure.Repository;

public class ClassRepository(IUnitOfWork unitOfWork) : CrudRepository<Class, Guid>(unitOfWork), IClassRepository
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    
    public override async Task<IEnumerable<Class>> GetAllAsync()
    {
        return await _unitOfWork.Context.Set<Class>().Include(s=>s.Students).ToListAsync();
    }

    public override async Task<Class> GetAsync(Guid id)
    {
        var result = await _unitOfWork.Context.Set<Class>()
            .Include(s => s.Students)
            .FirstOrDefaultAsync(s => s.Id == id);
        if (result == null)
        {
            throw new NotFoundException($"Entity of type {nameof(Class)} with ID {id} was not found.");
        }
        return result;
    }

    public override async Task<Class> UpdateAsync(Class entity)
    {
        await _unitOfWork.Context.Database.BeginTransactionAsync();
        try
        {
            var existingClass = await _unitOfWork.Context.Set<Class>()
                .Include(c => c.Students)
                .FirstOrDefaultAsync(c => c.Id == entity.Id);

            if (existingClass == null)
            {
                throw new Exception($"Class with ID {entity.Id} not found.");
            }

            _unitOfWork.Context.Entry(existingClass).CurrentValues.SetValues(entity);

            existingClass.Students.Clear();

            foreach (var student in entity.Students)
            {
                existingClass.Students.Add(student);
            }

            var result = await _unitOfWork.Context.SaveChangesAsync();

            if (result == 0)
            {
                throw new Exception($"Failed to update entity of type {nameof(Class)}.");
            }
            await _unitOfWork.CommitAsync();
            
            return existingClass;
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            throw new Exception("Transaction failed during deleting entities.", ex);
        }
    }
}