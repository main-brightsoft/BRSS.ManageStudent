namespace BRSS.ManageStudent.Application.DTO;

public class StudentDTO
{
    public Guid Id { get; set; }

    public string FullName { get; set; }

    public DateTime? DayOfBirth { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Address { get; set; }
    
}