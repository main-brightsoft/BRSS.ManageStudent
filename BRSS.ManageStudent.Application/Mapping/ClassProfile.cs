using AutoMapper;
using BRSS.ManageStudent.Application.DTO;
using BRSS.ManageStudent.Domain.Entity;

namespace BRSS.ManageStudent.Application.Mapping;

public class ClassProfile: Profile
{
    public ClassProfile()
    {
        CreateMap<Class, ClassDTO>();
        CreateMap<ClassCreateDTO, Class>();
        CreateMap<ClassUpdateDTO, Class>();
    }
}