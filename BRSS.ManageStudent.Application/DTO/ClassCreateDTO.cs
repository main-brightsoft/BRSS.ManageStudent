namespace BRSS.ManageStudent.Application.DTO;

public class ClassCreateDTO
{
    public string? ClassName { get; set; }
        
    public ICollection<Guid> StudentIds { get; set; } = new List<Guid>();
}