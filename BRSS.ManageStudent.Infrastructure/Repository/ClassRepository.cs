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
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            var existingClass = await _unitOfWork.Context.Set<Class>().Include(c => c.Students).FirstOrDefaultAsync(c => c.Id == entity.Id);
        
            if (existingClass == null)
            {
                throw new Exception($"Class with ID {entity.Id} not found.");
            }

            _unitOfWork.Context.Entry(existingClass).CurrentValues.SetValues(entity);

            existingClass.Students.Clear();
            existingClass.Students.AddRange(entity.Students);
            
            await _unitOfWork.Context.SaveChangesAsync();
            await _unitOfWork.CommitAsync();
            
            return entity;
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            throw new Exception("Transaction failed during deleting entities.", ex);
        }
    }
    
    public override async Task DeleteAsync(Guid id)
    {
        var entity = await _unitOfWork.Context.Set<Class>()
            .Include(s => s.Students)
            .FirstOrDefaultAsync(s => s.Id == id);
        if (entity == null) 
        {
            throw new Exception($"Entity of type {nameof(Class)} with ID {id} was not found for deletion.");
        }
        entity.Students.Clear();
        _unitOfWork.Context.Set<Class>().Remove(entity);
        var result = await _unitOfWork.Context.SaveChangesAsync();
            
        if (result == 0) 
        {
            throw new Exception($"Failed to delete entity of type {nameof(Class)}.");
        }
    }
    
    public override async Task DeleteManyAsync(List<Guid> ids)
    {
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            var entitiesToDelete = await _unitOfWork.Context.Set<Class>()
                .Where(e => ids.Contains(e.Id))
                .Include(s => s.Students)
                .ToListAsync();

            if (!entitiesToDelete.Any())
            {
                throw new NotFoundException("No entities found to delete.");
            }
            foreach (var classEntity in entitiesToDelete)
            {
                classEntity.Students.Clear();
                _unitOfWork.Context.Set<Class>().Remove(classEntity);
            }

            var result = await _unitOfWork.Context.SaveChangesAsync();
        
            if (result == 0)
            {
                throw new Exception("Failed to delete the specified entities.");
            }

            await _unitOfWork.CommitAsync();
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            throw new Exception("Transaction failed during deleting entities.", ex);
        }
    }
}