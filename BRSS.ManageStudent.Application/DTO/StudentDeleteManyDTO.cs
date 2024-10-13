namespace BRSS.ManageStudent.Application.DTO;

public class StudentDeleteManyDTO
{
    public List<Guid> Ids { get; set; } = new List<Guid>();
}