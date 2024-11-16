using AutoMapper;
using RealtorMVC.DAL.Entities;
using RealtorMVC.Models.User;

namespace RealtorMVC.BLL.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserModel>();
        CreateMap<UpdateUserModel, User>();
    }
}