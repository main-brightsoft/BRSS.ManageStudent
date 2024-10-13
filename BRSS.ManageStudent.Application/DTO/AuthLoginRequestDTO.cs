using System.ComponentModel.DataAnnotations;

namespace BRSS.ManageStudent.Application.DTO;

public class AuthLoginRequestDTO
{
    [Required]
    public string? UserName { get; set; }
    [Required]
    public string? Password { get; set; }
}