using BRSS.ManageStudent.Domain.Entity;
using BRSS.ManageStudent.Domain.Repository.Base;

namespace BRSS.ManageStudent.Domain.Repository;

public interface IUserRepository: ICrudRepository<ApplicationUser, string>
{
    Task<ApplicationUser> GetByEmailAsync(string email);
    Task<ApplicationUser> GetByUserNameAsync(string userName);
    Task<bool> IsEmailConfirmedAsync(string email);
    Task<bool> check();
}