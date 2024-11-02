using AutoMapper;
using RealtorMVC.DAL.Entities;
using RealtorMVC.Models.Auth;

namespace RealtorMVC.BLL.Profiles;

public class AuthProfile : Profile
{
    public AuthProfile()
    {
        CreateMap<SignUpModel, User>();
        CreateMap<SignInModel, User>();
    }
}
