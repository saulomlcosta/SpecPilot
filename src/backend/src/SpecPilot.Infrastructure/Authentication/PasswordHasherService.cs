using Microsoft.AspNetCore.Identity;
using SpecPilot.Application.Abstractions.Auth;
using SpecPilot.Domain.Entities;

namespace SpecPilot.Infrastructure.Authentication;

public class PasswordHasherService : IPasswordHasherService
{
    private readonly PasswordHasher<User> _passwordHasher = new();

    public string HashPassword(User user, string password) => _passwordHasher.HashPassword(user, password);

    public PasswordVerificationResult VerifyHashedPassword(User user, string hashedPassword, string providedPassword)
        => _passwordHasher.VerifyHashedPassword(user, hashedPassword, providedPassword);
}
