namespace BRSS.ManageStudent.Application.DTO;

public class ClassUpdateDTO
{
    public string? ClassName { get; set; }
    public List<Guid> StudentIds { get; set; } = new List<Guid>();
}