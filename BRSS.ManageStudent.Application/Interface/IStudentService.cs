using BRSS.ManageStudent.Application.DTO;
using BRSS.ManageStudent.Application.Interface.Base;

namespace BRSS.ManageStudent.Application.Interface;

public interface IStudentService:ICrudService<StudentDTO, StudentCreateDTO, StudentUpdateDTO, Guid>
{
    
}