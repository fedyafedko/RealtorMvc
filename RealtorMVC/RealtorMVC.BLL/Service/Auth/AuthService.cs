using RealtorMVC.BLL.Interfaces;
using RealtorMVC.DAL.Entities;
using RealtorMVC.Models.Auth;
using Microsoft.AspNetCore.Identity;
using RealtorMVC.Common.Exceptions;
using RealtorMVC.Common.Configs;
using AutoMapper;
using RealtorMVC.FluentEmail.MessageBase;
using RealtorMVC.Abstraction.FluentEmail;

namespace RealtorMVC.BLL.Service.Auth;

public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly ITokenService _tokenService;
    private readonly IEmailConfirmationService _emailConfirmationService;
    private readonly IEmailService _emailService;
    private readonly JwtConfig _jwtConfig;
    private readonly IMapper _mapper;

    public AuthService(
        UserManager<User> userManager,
        ITokenService tokenService,
        IEmailConfirmationService emailConfirmationService,
        IEmailService emailService,
        JwtConfig jwtConfig,
        IMapper mapper)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _emailConfirmationService = emailConfirmationService;
        _emailService = emailService;
        _jwtConfig = jwtConfig;
        _mapper = mapper;
    }

    public async Task<AuthSuccessModel> SignInAsync(SignInModel model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email)
            ?? throw new NotFoundException($"Unable to find user by specified email. Email: {model.Email}");

        if (!user.EmailConfirmed)
            throw new ConfirmedEmailException("Email is not confirmed");

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, model.Password);

        if (!isPasswordValid)
            throw new IncorrectParametersException($"User input incorrect password. Password: {model.Password}");

        return await _tokenService.GetAuthTokensAsync(user);
    }

    public async Task<RegisterModel> SignUpAsync(SignUpModel model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);

        if (user != null)
            throw new AlreadyExistsException($"User with specified email already exists. Email: {model.Email}");

        user = _mapper.Map<User>(model);
        user.UserName = model.Email;

        var result = await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
        {
            throw new UserManagerException($"User manager operation failed:\n", result.Errors);
        }

        var role = model.Role;

        var roleResult = await _userManager.AddToRoleAsync(user, role);

        if (!roleResult.Succeeded)
        {
            throw new UserManagerException($"User manager operation failed:\n", result.Errors);
        }

        var generatedCode = await _emailConfirmationService.GenerateEmailCodeAsync(user.Id);

        await _emailService.SendAsync(new ConfirmedEmailMessage { Recipient = user.Email!, Code = generatedCode });

        return new RegisterModel { UserId = user.Id };
    }
}