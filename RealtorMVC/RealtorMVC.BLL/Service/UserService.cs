using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using RealtorMVC.BLL.Interfaces;
using RealtorMVC.Common.Configs;
using RealtorMVC.Common.Exceptions;
using RealtorMVC.DAL.Entities;
using RealtorMVC.Models.User;

namespace RealtorMVC.BLL.Service;
public class UserService : IUserService
{
    private readonly UserManager<User> _userManager;
    private readonly UserAvatarConfig _userAvatarConfig;
    private readonly IWebHostEnvironment _env;
    private readonly IMapper _mapper;

    public UserService(
        UserManager<User> userManager,
        IMapper mapper,
        UserAvatarConfig userAvatarConfig,
        IWebHostEnvironment env)
    {
        _userManager = userManager;
        _mapper = mapper;
        _userAvatarConfig = userAvatarConfig;
        _env = env;
    }

    public async Task<bool> DeleteAvatarAsync(Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString())
                    ?? throw new NotFoundException("User not found");

        var wwwPath = _env.ContentRootPath;
        var path = Path.Combine(wwwPath, _userAvatarConfig.Folder);

        var file = Directory.GetFiles(path).FirstOrDefault(x => x.Contains(userId.ToString()));

        if (string.IsNullOrEmpty(file))
            throw new NotFoundException("File not found");

        File.Delete(file);

        user.Avatar = string.Empty;

        var result = await _userManager.UpdateAsync(user);

        return result.Succeeded;
    }

    public async Task<UserModel> EditProfileAsync(Guid userId, UpdateUserModel dto)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString())
           ?? throw new NotFoundException("User not found");

        _mapper.Map(dto, user);

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            throw new UserManagerException("User errors:", result.Errors);
        }

        return _mapper.Map<UserModel>(user);
    }

    public async Task<UserModel> GetUserAsync(Guid userId)
    {
        var entity = await _userManager.FindByIdAsync(userId.ToString())
                    ?? throw new NotFoundException("User not found");

        var file = Directory.GetFiles(Path.Combine(_env.ContentRootPath, _userAvatarConfig.Folder))
            .Select(x => Path.GetFileName(x))
            .FirstOrDefault(x => x.Contains(userId.ToString()));

        var role = await _userManager.GetRolesAsync(entity);

        var user = _mapper.Map<UserModel>(entity);

        if (!entity.Avatar.IsNullOrEmpty() && !file.IsNullOrEmpty())
            user.AvatarUrl = string.Format(_userAvatarConfig.Path, file);
        else
            user.AvatarUrl = string.Empty;

        user.Role = role.FirstOrDefault()!;

        return user;
    }

    public async Task<AvatarResponse> UploadAvatarAsync(Guid userId, IFormFile avatar)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString())
            ?? throw new NotFoundException("User not found");

        var contetntPath = _env.ContentRootPath;
        var path = Path.Combine(contetntPath, _userAvatarConfig.Folder);

        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        if (!string.IsNullOrEmpty(user.Avatar))
        {
            var oldAvatarPath = Path.Combine(path, user.Avatar);
            if (File.Exists(oldAvatarPath))
            {
                File.Delete(oldAvatarPath);
            }
        }

        var ext = Path.GetExtension(avatar.FileName);

        if (!_userAvatarConfig.FileExtensions.Contains(ext))
            throw new IncorrectParametersException("Invalid file extension");

        var newFileName = $"{userId}{ext}";
        var filePath = Path.Combine(path, newFileName);

        using var stream = new FileStream(filePath, FileMode.Create);
        await avatar.CopyToAsync(stream);

        user.Avatar = newFileName;
        await _userManager.UpdateAsync(user);

        var result = new AvatarResponse
        {
            Path = string.Format(_userAvatarConfig.Path, newFileName)
        };

        return result;
    }
}
