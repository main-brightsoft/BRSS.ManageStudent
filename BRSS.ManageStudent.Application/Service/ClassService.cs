using AutoMapper;
using BRSS.ManageStudent.Application.DTO;
using BRSS.ManageStudent.Application.Interface;
using BRSS.ManageStudent.Application.Service.Base;
using BRSS.ManageStudent.Domain.Entity;
using BRSS.ManageStudent.Domain.Repository;

namespace BRSS.ManageStudent.Application.Service;

public class ClassService:CrudService<ClassDTO, ClassCreateDTO, ClassUpdateDTO, Class, Guid>, IClassService
{
    private readonly IMapper _mapper;
    private readonly IStudentRepository _studentRepository;

    public ClassService(IClassRepository classRepository, IStudentRepository studentRepository, IMapper mapper) : base(classRepository)
    {
        _mapper = mapper;
        _studentRepository = studentRepository;
    }
    protected override ClassDTO MapEntityToEntityDto(Class entity)
    {
        return _mapper.Map<ClassDTO>(entity);
    }

    protected override async Task<Class> MapEntityCreateDtoToEntity(ClassCreateDTO entityCreateDTO)
    {
        var @class = _mapper.Map<Class>(entityCreateDTO);
        @class.Id = Guid.NewGuid();
        @class.Students = await FetchStudentsByIds(entityCreateDTO.StudentIds);
        return @class;
    }

    protected override async Task<Class> MapEntityUpdateDtoToEntity(Guid id ,ClassUpdateDTO entityUpdateDTO)
    {
        var @class = _mapper.Map<Class>(entityUpdateDTO);
        @class.Id = id;
        @class.Students = await FetchStudentsByIds(entityUpdateDTO.StudentIds);
        return @class;
    }
    private async Task<List<Student>> FetchStudentsByIds(IEnumerable<Guid> studentIds)
    {
        var students = await _studentRepository.GetByIdsAsync(studentIds);
        return students.ToList();
    }
}