using System.IdentityModel.Tokens.Jwt;

namespace RealtorMVC.Extentions;

public static class HttpContextExtension
{
    public static Guid GetUserId(this HttpContext context)
    {
        var authHeader = context.Request.Headers["Authorization"].ToString();
        var token = authHeader.StartsWith("Bearer ") ? authHeader.Substring("Bearer ".Length).Trim() : authHeader;

        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(token) as JwtSecurityToken;


        var claim = jsonToken?.Claims.FirstOrDefault(c => c.Type == "id");

        if (claim == null)
            throw new Exception("Unauthorized");

        return new Guid(claim.Value);

    }
}
