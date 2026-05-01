using MediatR;
using SpecPilot.Application.Auth.Common;
using SpecPilot.Domain.Common;

namespace SpecPilot.Application.Auth.Login;

public class LoginUserCommand : IRequest<Result<AuthResponse>>
{
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}
