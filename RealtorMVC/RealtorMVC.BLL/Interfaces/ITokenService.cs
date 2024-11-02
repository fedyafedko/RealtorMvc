using RealtorMVC.DAL.Entities;
using RealtorMVC.Models.Auth;
using System.Security.Claims;

namespace RealtorMVC.BLL.Interfaces;

public interface ITokenService
{
    Task<string> GenerateAccessTokenAsync(User user);
    Task<AuthSuccessModel> GetAuthTokensAsync(User user);
}