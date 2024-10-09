using BRSS.ManageStudent.Application.DTO;
using BRSS.ManageStudent.Application.Interface.Base;
using BRSS.ManageStudent.Domain.Entity;

namespace BRSS.ManageStudent.Application.Interface;

public interface IClassService: ICrudService<ClassDTO, ClassCreateDTO, ClassUpdateDTO, Guid>
{
        
}