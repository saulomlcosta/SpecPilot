using MediatR;
using SpecPilot.Application.Auth.Common;
using SpecPilot.Domain.Common;

namespace SpecPilot.Application.Auth.Register;

public class RegisterUserCommand : IRequest<Result<AuthResponse>>
{
    public string Name { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}
