using BRSS.ManageStudent.Application.DTO;
using BRSS.ManageStudent.Application.Interface;
using BRSS.ManageStudent.Application.Interface.Base;
using BRSS.ManageStudent.Controllers.Base;
using BRSS.ManageStudent.Domain.Entity;

namespace BRSS.ManageStudent.Controllers;

public class StudentController: CrudController<StudentDTO, StudentCreateDTO, StudentUpdateDTO, Guid>
{
    public StudentController(IStudentService service) : base(service)
    {
    }
}