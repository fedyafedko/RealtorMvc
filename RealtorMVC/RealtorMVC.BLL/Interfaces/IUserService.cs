using Microsoft.AspNetCore.Http;
using RealtorMVC.Models.User;

namespace RealtorMVC.BLL.Interfaces;

public interface IUserService
{
    Task<UserModel> GetUserAsync(Guid userId);
    Task<AvatarResponse> UploadAvatarAsync(Guid userId, IFormFile avatar);
    Task<bool> DeleteAvatarAsync(Guid userId);
    Task<UserModel> EditProfileAsync(Guid userId, UpdateUserModel dto);
}