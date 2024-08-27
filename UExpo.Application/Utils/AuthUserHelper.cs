using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using UExpo.Domain.Authentication;

namespace UExpo.Application.Utils;

public class AuthUserHelper
{
    private readonly IHttpContextAccessor _context;

    public AuthUserHelper(IHttpContextAccessor contextAccessor)
    {
        _context = contextAccessor;
    }

    public AuthenticatedUser GetUser()
    {
        ClaimsPrincipal user = _context.HttpContext.User;

        return new()
        {
            Id = Guid.Parse(user.GetJwtClaim<string>("id")!),
            Name = user.GetJwtClaim<string>("name")!,
            Email = user.GetJwtClaim<string>("email"),
            Type = user.GetJwtClaim<string>("type"),
        };
    }
}

public static class ClaimsPrincipalExtension
{
    public static T? GetJwtClaim<T>(this ClaimsPrincipal user, string claimType)
    {
        Claim? claim = user.Claims.FirstOrDefault(c => c.Type == claimType);

        if (claim is null) return default;

        return (T)Convert.ChangeType(claim.Value, typeof(T));
    }
}
