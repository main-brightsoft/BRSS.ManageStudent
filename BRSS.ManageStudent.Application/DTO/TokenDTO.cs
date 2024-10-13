namespace BRSS.ManageStudent.Application.DTO;

public class TokenDTO
{
    public string Type { get; set; } = "Bearer";
    public string AccessToken { get; set; }
    public string ExpiresAt { get; set; }
}