namespace BRSS.ManageStudent.Application.DTO;

public class ClassUpdateDTO
{
    public string? ClassName { get; set; }
    public ICollection<Guid> StudentIds { get; set; } = new List<Guid>();
}