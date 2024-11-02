using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using RealtorMVC.BLL.Interfaces;
using RealtorMVC.Common.Configs;
using RealtorMVC.DAL.Entities;
using RealtorMVC.Models.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RealtorMVC.BLL.Service.Auth;

public class TokenService : ITokenService
{
    private readonly UserManager<User> _userManager;
    private readonly JwtConfig _jwtConfig;

    public TokenService(
        UserManager<User> userManager,
        JwtConfig jwtConfig)
    {
        _userManager = userManager;
        _jwtConfig = jwtConfig;
    }

    public async Task<string> GenerateAccessTokenAsync(User user)
    {
        var jwtTokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_jwtConfig.Secret);

        var claims = new List<Claim>
        {
            new Claim("id", user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Sub, user.Email!),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var roles = await _userManager.GetRolesAsync(user);

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.Add(_jwtConfig.AccessTokenLifeTime),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Issuer = _jwtConfig.Issuer,
            Audience = _jwtConfig.Audience
        };

        var token = jwtTokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = jwtTokenHandler.WriteToken(token);
        return jwtToken;
    }

    public async Task<AuthSuccessModel> GetAuthTokensAsync(User user)
    {
        return new AuthSuccessModel
        {
            AccessToken = await GenerateAccessTokenAsync(user),
        };
    }
}
