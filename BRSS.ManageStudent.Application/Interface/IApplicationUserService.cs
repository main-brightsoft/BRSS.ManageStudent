using BRSS.ManageStudent.Application.DTO;
using BRSS.ManageStudent.Application.Interface.Base;
using BRSS.ManageStudent.Domain.Repository.Base;

namespace BRSS.ManageStudent.Application.Interface;

public interface IApplicationUserService: ICrudService<ApplicationUserDTO, ApplicationUserCreateDTO, ApplicationUserUpdateDTO, Guid>
{
    
}