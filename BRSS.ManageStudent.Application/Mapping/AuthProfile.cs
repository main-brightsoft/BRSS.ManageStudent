using AutoMapper;
using BRSS.ManageStudent.Application.DTO;
using BRSS.ManageStudent.Domain.Entity;
using Microsoft.AspNetCore.Identity;

namespace BRSS.ManageStudent.Application.Mapping;

public class AuthProfile: Profile
{
    public AuthProfile(){
        CreateMap<ApplicationUser, AuthLoginDTO>();
        CreateMap<AuthRegisterRequestDTO, ApplicationUser>();
    }
}