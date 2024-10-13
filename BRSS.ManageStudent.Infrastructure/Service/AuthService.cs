using AutoMapper;
using BRSS.ManageStudent.Application.DTO;
using BRSS.ManageStudent.Application.Interface;
using BRSS.ManageStudent.Domain.Entity;
using BRSS.ManageStudent.Domain.Exception;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace BRSS.ManageStudent.Application.Service;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;
    private readonly IEmailService _emailService;
    private readonly ILogger<AuthService> _logger;

    public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, 
        ITokenService tokenService, IMapper mapper, IEmailService emailService, ILogger<AuthService> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _mapper = mapper;
        _emailService = emailService;
        _logger = logger;
    }


    public async Task<AuthLoginDTO> Login(AuthLoginRequestDTO request)
    {
        var user = await _userManager.FindByNameAsync(request.UserName!);
    
        if (user == null)
        {
            throw new NotFoundException("User account has not been registered.");
        }
        
        if (!user.EmailConfirmed)
        {
            await SendConfirmationEmailAsync(user);
            throw new EmailNotConfirmedException("Email not confirmed. Please verify your email address.");
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password!, false);

        if (!result.Succeeded)
        {
            throw new UnauthorizedAccessException("Invalid username or password");
        }

        return MapUserToAuthLoginDTO(user);
    }

    public async Task Register(AuthRegisterRequestDTO request)
    {
        var user = MapAuthRegisterRequestDTOToUser(request);

        var result = await _userManager.CreateAsync(user, request.Password);
        await _userManager.AddToRoleAsync(user, "User");
        
        if (!result.Succeeded)
        {
            throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        await SendConfirmationEmailAsync(user);
    }
    
    private async Task SendConfirmationEmailAsync(ApplicationUser user)
    {
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var recipientName = $"{user.FirstName} {user.LastName}";

        _ = Task.Run(async () =>
        {
            try
            {
                await _emailService.SendConfirmationEmailAsync(user.Email!, recipientName, token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send confirmation email to {Email}", user.Email);
            }
        });
    }

    private AuthLoginDTO MapUserToAuthLoginDTO(ApplicationUser user)
    {
        var userDto = _mapper.Map<ApplicationUserDTO>(user);
        var authLoginDto = new AuthLoginDTO();
        var token = _tokenService.GenerateToken(user);
        authLoginDto.User = userDto;
        authLoginDto.Token = token;
        return authLoginDto;
    }

    private ApplicationUser MapAuthRegisterRequestDTOToUser(AuthRegisterRequestDTO authRegisterRequestDto)
    {
        return _mapper.Map<ApplicationUser>(authRegisterRequestDto);
    }
}
