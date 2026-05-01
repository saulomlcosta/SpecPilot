using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SpecPilot.Application.Abstractions.Auth;
using SpecPilot.Application.Abstractions.Persistence;
using SpecPilot.Application.Auth.Common;
using SpecPilot.Domain.Common;

namespace SpecPilot.Application.Auth.Login;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, Result<AuthResponse>>
{
    private readonly IApplicationDbContext _context;
    private readonly IPasswordHasherService _passwordHasherService;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public LoginUserCommandHandler(
        IApplicationDbContext context,
        IPasswordHasherService passwordHasherService,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _context = context;
        _passwordHasherService = passwordHasherService;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<Result<AuthResponse>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var normalizedEmail = request.Email.Trim().ToLowerInvariant();

        var user = await _context.Users
            .FirstOrDefaultAsync(x => x.Email == normalizedEmail, cancellationToken);

        if (user is null)
        {
            return Result.Failure<AuthResponse>(Error.Unauthorized(
                "auth.invalid_credentials",
                "Email ou senha invalidos."));
        }

        var passwordResult = _passwordHasherService.VerifyHashedPassword(user, user.PasswordHash, request.Password);

        if (passwordResult == PasswordVerificationResult.Failed)
        {
            return Result.Failure<AuthResponse>(Error.Unauthorized(
                "auth.invalid_credentials",
                "Email ou senha invalidos."));
        }

        return Result.Success(new AuthResponse
        {
            Token = _jwtTokenGenerator.GenerateToken(user),
            User = new UserResponse
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
            }
        });
    }
}
