using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealtorMVC.BLL.Interfaces;
using RealtorMVC.Extentions;
using RealtorMVC.Models.User;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace RealtorMVC.Controllers;

public class UserController : Controller
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> GetUser()
    {
        var userId = HttpContext.GetUserId();
        var user = await _userService.GetUserAsync(userId);
        return Ok(user);
    }

    [HttpPost]
    public async Task<IActionResult> UploadAvatar(IFormFile avatar)
    {
        var userId = HttpContext.GetUserId();
        var response = await _userService.UploadAvatarAsync(userId, avatar);
        return Ok(response);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteAvatar()
    {
        var userId = HttpContext.GetUserId();
        var result = await _userService.DeleteAvatarAsync(userId);
        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> EditProfile([FromBody] UpdateUserModel dto)
    {
        var userId = HttpContext.GetUserId();
        var user = await _userService.EditProfileAsync(userId, dto);
        return Ok(user);
    }
}
