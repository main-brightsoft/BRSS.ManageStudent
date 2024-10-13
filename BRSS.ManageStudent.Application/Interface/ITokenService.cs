using BRSS.ManageStudent.Domain.Entity;
using Microsoft.AspNetCore.Identity;

namespace BRSS.ManageStudent.Application.Interface;

public interface ITokenService
{
    string GenerateToken(ApplicationUser user);
}