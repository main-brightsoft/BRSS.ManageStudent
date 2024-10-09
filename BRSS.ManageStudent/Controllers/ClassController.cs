using BRSS.ManageStudent.Application.DTO;
using BRSS.ManageStudent.Application.Interface;
using BRSS.ManageStudent.Application.Interface.Base;
using BRSS.ManageStudent.Controllers.Base;

namespace BRSS.ManageStudent.Controllers;

public class ClassController: CrudController<ClassDTO, ClassCreateDTO, ClassUpdateDTO, Guid>
{
    public ClassController(IClassService service) : base(service)
    {
    }
}