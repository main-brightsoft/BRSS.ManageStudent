using BRSS.ManageStudent.Application.DTO;

namespace BRSS.ManageStudent.Application.Interface;

public interface IAuthService
{
    Task<AuthLoginDTO> Login(AuthLoginRequestDTO request);
    Task Register(AuthRegisterRequestDTO request);
}