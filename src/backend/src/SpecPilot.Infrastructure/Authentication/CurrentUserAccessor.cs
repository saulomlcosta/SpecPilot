using Microsoft.AspNetCore.Http;
using SpecPilot.Application.Abstractions.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SpecPilot.Infrastructure.Authentication;

public class CurrentUserAccessor : ICurrentUserAccessor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserAccessor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid? UserId
    {
        get
        {
            var user = _httpContextAccessor.HttpContext?.User;
            var userId =
                user?.FindFirstValue(ClaimTypes.NameIdentifier) ??
                user?.FindFirstValue(JwtRegisteredClaimNames.Sub) ??
                user?.FindFirstValue("sub");

            return Guid.TryParse(userId, out var parsedUserId) ? parsedUserId : null;
        }
    }
}
