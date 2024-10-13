using BRSS.ManageStudent.Application.DTO;
using BRSS.ManageStudent.Application.Interface;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace BRSS.ManageStudent.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController: ControllerBase
{
    private  readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AuthLoginRequestDTO request)
    {
        var loginDto = await _authService.Login(request);
        return Ok(loginDto);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] AuthRegisterRequestDTO request)
    {
        await _authService.Register(request);
        return Ok();
    }
}