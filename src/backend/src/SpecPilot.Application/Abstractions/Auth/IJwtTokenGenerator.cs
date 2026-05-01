using SpecPilot.Domain.Entities;

namespace SpecPilot.Application.Abstractions.Auth;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}
