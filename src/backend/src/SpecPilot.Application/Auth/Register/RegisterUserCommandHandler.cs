using MediatR;
using Microsoft.EntityFrameworkCore;
using SpecPilot.Application.Abstractions.Auth;
using SpecPilot.Application.Abstractions.Persistence;
using SpecPilot.Application.Auth.Common;
using SpecPilot.Domain.Common;
using SpecPilot.Domain.Entities;

namespace SpecPilot.Application.Auth.Register;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Result<AuthResponse>>
{
    private readonly IApplicationDbContext _context;
    private readonly IPasswordHasherService _passwordHasherService;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public RegisterUserCommandHandler(
        IApplicationDbContext context,
        IPasswordHasherService passwordHasherService,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _context = context;
        _passwordHasherService = passwordHasherService;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<Result<AuthResponse>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var normalizedEmail = request.Email.Trim().ToLowerInvariant();

        var emailAlreadyExists = await _context.Users
            .AnyAsync(x => x.Email == normalizedEmail, cancellationToken);

        if (emailAlreadyExists)
        {
            return Result.Failure<AuthResponse>(Error.Conflict(
                "auth.email_already_registered",
                "Ja existe um usuario cadastrado com este email."));
        }

        var user = new User
        {
            Name = request.Name.Trim(),
            Email = normalizedEmail
        };

        user.PasswordHash = _passwordHasherService.HashPassword(user, request.Password);

        _context.Users.Add(user);
        await _context.SaveChangesAsync(cancellationToken);

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
