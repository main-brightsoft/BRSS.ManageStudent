using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BRSS.ManageStudent.Application.Interface;
using BRSS.ManageStudent.Domain.Entity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

public class TokenService: ITokenService
{
    private readonly string _issuer;
    private readonly string _audience;
    private readonly string _key;
    private readonly int _accessTokenExpires;
    private const int AccessTokenExpiry = 60;
    public TokenService(IConfiguration configuration)
    {
        _issuer = configuration["Jwt:Issuer"] ?? throw new ArgumentNullException("Issuer not configured");
        _audience = configuration["Jwt:Audience"] ?? throw new ArgumentNullException("Audience not configured");
        _key = configuration["Jwt:Key"] ?? throw new ArgumentNullException("Key not configured");
        _accessTokenExpires = int.TryParse(configuration["Jwt:AccessTokenExpires"], out var accessTokenExpiry) ? accessTokenExpiry : AccessTokenExpiry;
    }

    public string GenerateToken(ApplicationUser user)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email ?? throw new InvalidOperationException("Email is required")),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_accessTokenExpires),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
