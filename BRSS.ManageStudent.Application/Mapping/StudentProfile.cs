using AutoMapper;
using BRSS.ManageStudent.Application.DTO;
using BRSS.ManageStudent.Domain.Entity;

namespace BRSS.ManageStudent.Application.Mapping;

public class StudentProfile: Profile
{
    public StudentProfile()
    {
        CreateMap<Student, StudentDTO>();
        CreateMap<StudentCreateDTO, Student>();
        CreateMap<StudentUpdateDTO, Student>();
    }
}