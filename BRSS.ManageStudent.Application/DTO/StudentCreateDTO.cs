using System.ComponentModel.DataAnnotations;

namespace BRSS.ManageStudent.Application.DTO;

public class StudentCreateDTO
{
    [Required]
    [MaxLength(100)]
    public string? FullName { get; set; }

    public DateTime? DayOfBirth { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Address { get; set; }
}