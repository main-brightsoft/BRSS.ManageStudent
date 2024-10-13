namespace BRSS.ManageStudent.Application.DTO;

public class ClassCreateDTO
{
    public string? ClassName { get; set; }
        
    public List<Guid> StudentIds { get; set; } = new List<Guid>();
}