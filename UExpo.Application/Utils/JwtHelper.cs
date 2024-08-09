using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UExpo.Domain.Admins;
using UExpo.Domain.Shared;
using UExpo.Domain.Users;

namespace UExpo.Application.Utils;

public static class JwtHelper
{
    public static string GenerateJwtToken(BaseModel model, IConfiguration config)
    {
        Claim[] claims = [];

        if (model is User user)
            claims = BuildUserClaims(user);

        if (model is Admin admin)
            claims = BuildAdminsClaims(admin);

        var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            config["Jwt:Issuer"],
            config["Jwt:Audience"],
            claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(config["Jwt:ExpiresInMinutes"])),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static Claim[] BuildUserClaims(User user)
    {
        return
        [
            new Claim(JwtRegisteredClaimNames.Sub, user.Name),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("Email", user.Email),
            new Claim("UserType", user.Type.ToString())
        ];
    }

    private static Claim[] BuildAdminsClaims(Admin admin)
    {
        return
        [
            new Claim(JwtRegisteredClaimNames.Sub, admin.Name),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("type", admin.Type.ToString())
        ];
    }
}
