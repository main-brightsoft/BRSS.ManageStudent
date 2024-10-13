using BRSS.ManageStudent.Application.UnitOfWork;
using BRSS.ManageStudent.Domain.Entity.Base;
using BRSS.ManageStudent.Domain.Exception;
using BRSS.ManageStudent.Domain.Repository.Base;
using Microsoft.EntityFrameworkCore;

namespace BRSS.ManageStudent.Infrastructure.Repository.Base
{
   public abstract class CrudRepository<TEntity, TKey> : ReadOnlyRepository<TEntity, TKey>, ICrudRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
    {
        private readonly IUnitOfWork _unitOfWork;
        protected CrudRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public virtual async Task<TEntity> AddAsync(TEntity entity)
        {
            await _unitOfWork.Context.Set<TEntity>().AddAsync(entity);
            var result = await _unitOfWork.Context.SaveChangesAsync();
            
            if (result == 0) 
            {
                throw new Exception($"Failed to add entity of type {typeof(TEntity).Name}.");
            }

            return entity;
        }

        public virtual async Task<TEntity> UpdateAsync(TEntity entity)
        {
            _unitOfWork.Context.Set<TEntity>().Update(entity);
            var result = await _unitOfWork.Context.SaveChangesAsync();
            
            if (result == 0) 
            {
                throw new Exception($"Failed to update entity of type {typeof(TEntity).Name}.");
            }

            return entity;
        }

        public virtual async Task DeleteAsync(TKey id)
        {
            var entity = await _unitOfWork.Context.Set<TEntity>().FindAsync(id);
            if (entity == null) 
            {
                throw new Exception($"Entity of type {typeof(TEntity).Name} with ID {id} was not found for deletion.");
            }
            _unitOfWork.Context.Set<TEntity>().Remove(entity);
            var result = await _unitOfWork.Context.SaveChangesAsync();
            
            if (result == 0) 
            {
                throw new Exception($"Failed to delete entity of type {typeof(TEntity).Name}.");
            }
        }
        public abstract Task DeleteManyAsync(List<TKey> ids);

    }
}
