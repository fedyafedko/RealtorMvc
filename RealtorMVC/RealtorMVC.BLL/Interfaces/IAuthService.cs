using RealtorMVC.Models.Auth;

namespace RealtorMVC.BLL.Interfaces;

public interface IAuthService
{
    Task<AuthSuccessModel> SignInAsync(SignInModel model);
    Task<RegisterModel> SignUpAsync(SignUpModel model);
}