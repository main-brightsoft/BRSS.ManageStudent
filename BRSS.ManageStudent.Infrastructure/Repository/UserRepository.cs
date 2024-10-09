using BRSS.ManageStudent.Application.UnitOfWork;
using BRSS.ManageStudent.Domain.Entity;
using BRSS.ManageStudent.Domain.Exception;
using BRSS.ManageStudent.Domain.Repository;
using BRSS.ManageStudent.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BRSS.ManageStudent.Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;

        public UserRepository(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllAsync()
        {
            return await _userManager.Users.ToListAsync();
        }

        public async Task<ApplicationUser> GetAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                throw new NotFoundException($"User with ID {id} was not found.");
            }

            return user;
        }

        public async Task<ApplicationUser> AddAsync(ApplicationUser entity)
        {
            var result = await _userManager.CreateAsync(entity, entity.PasswordHash ?? throw new InvalidOperationException());
            if (!result.Succeeded) 
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Failed to create user: {errors}");
            }
            return entity;
        }

        public async Task<ApplicationUser> UpdateAsync(ApplicationUser entity)
        {
            var result = await _userManager.UpdateAsync(entity);
            if (!result.Succeeded) 
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Failed to update user: {errors}");
            }
            return entity;
        }

        public async Task DeleteAsync(string id)
        {
            var user = await GetAsync(id);
            if (user == null) throw new NotFoundException($"User with ID {id} was not found.");

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Failed to delete user: {errors}");
            }
        }

        public async Task DeleteManyAsync(IEnumerable<string> ids)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                foreach (var id in ids)
                {
                    await DeleteAsync(id);
                }

                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                throw new Exception("Transaction failed during DeleteMany operation", ex);
            }
        }

        public Task<ApplicationUser> GetByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationUser> GetByUserNameAsync(string userName)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsEmailConfirmedAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task<bool> check()
        {
            throw new NotImplementedException();
        }
    }
}
