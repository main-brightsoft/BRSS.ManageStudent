using AutoMapper;
using BRSS.ManageStudent.Application.DTO;
using BRSS.ManageStudent.Application.Interface;
using BRSS.ManageStudent.Application.Service.Base;
using BRSS.ManageStudent.Domain.Entity;
using BRSS.ManageStudent.Domain.Repository;

namespace BRSS.ManageStudent.Application.Service;

public class StudentService: CrudService<StudentDTO, StudentCreateDTO, StudentUpdateDTO, Student, Guid>, IStudentService
{
    private readonly IMapper _mapper;

    public StudentService(IStudentRepository studentRepository, IMapper mapper) : base(studentRepository)
    {
        _mapper = mapper;
    }
    protected override StudentDTO MapEntityToEntityDto(Student entity)
    {
        return _mapper.Map<StudentDTO>(entity);
    }

    protected override Task<Student> MapEntityCreateDtoToEntity(StudentCreateDTO entityCreateDTO)
    {
        var student = _mapper.Map<Student>(entityCreateDTO);
        student.Id = Guid.NewGuid();
        return Task.FromResult(student);
    }

    protected override Task<Student> MapEntityUpdateDtoToEntity(Guid id, StudentUpdateDTO entityUpdateDTO)
    {
        var student = _mapper.Map<Student>(entityUpdateDTO);
        student.Id = id;
        return Task.FromResult(student);
    }
}