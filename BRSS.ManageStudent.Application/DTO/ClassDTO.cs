namespace BRSS.ManageStudent.Application.DTO;

public class ClassDTO
{
    public Guid Id { get; set; }
        
    public string? ClassName { get; set; }
        
    public ICollection<StudentDTO> Students { get; set; } = new List<StudentDTO>();
}