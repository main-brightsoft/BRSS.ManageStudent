using AutoMapper;
using BRSS.ManageStudent.Application.DTO;
using Microsoft.AspNetCore.Identity;

namespace BRSS.ManageStudent.Application.Mapping;

public class ApplicationUserProfile: Profile
{
    public ApplicationUserProfile()
    {
        // CreateMap<IdentityUser, ApplicationUserDTO>();
    }
}