using AutoMapper;
using BRSS.ManageStudent.Application.DTO;
using BRSS.ManageStudent.Application.Interface;
using BRSS.ManageStudent.Domain.Entity;
using BRSS.ManageStudent.Infrastructure.Config;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace BRSS.ManageStudent.Infrastructure.Service;

public class OAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly GoogleOAuthConfig _googleOAuthConfig;
    private readonly IMapper _mapper;
    private readonly ITokenService _tokenService;

    public OAuthService(IOptions<GoogleOAuthConfig> googleOAuthConfig, IMapper mapper, ITokenService tokenService, UserManager<ApplicationUser> userManager)
    {
        _mapper = mapper;
        _tokenService = tokenService;
        _userManager = userManager;
        _googleOAuthConfig = googleOAuthConfig.Value;
    }

    public async Task<AuthLoginDTO> LoginGoogle(string idToken)
    {
        var payload = await ValidateGoogleToken(idToken);
        if (payload == null)
        {
            throw new Exception("Google oauth token is invalid");
        }
        var existingUser = await _userManager.FindByEmailAsync(payload.Email);
        if (existingUser != null)
        {
            return MapUserToAuthLoginDTO(existingUser);
        }
        var newUser = new ApplicationUser
        {
            Email = payload.Email,
            UserName = payload.Email,
            FirstName = payload.Name,
            EmailConfirmed = true,
        };
        var result = await _userManager.CreateAsync(newUser);
        if (!result.Succeeded)
        {
            throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
        }
        return MapUserToAuthLoginDTO(newUser);
    }
    private async Task<GoogleJsonWebSignature.Payload?> ValidateGoogleToken(string token)
    {
        try
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = new List<string> { _googleOAuthConfig.ClientId }
            };
            return await GoogleJsonWebSignature.ValidateAsync(token, settings);
        }
        catch (InvalidJwtException ex)
        {
            throw new Exception("Invalid Google JWT", ex);
        }
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
}
