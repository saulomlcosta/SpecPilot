using Microsoft.AspNetCore.Identity;
using SpecPilot.Application.Abstractions.Auth;
using SpecPilot.Domain.Entities;

namespace SpecPilot.UnitTests.Auth;

internal sealed class TestJwtTokenGenerator : IJwtTokenGenerator
{
    public string GenerateToken(User user) => "fake-jwt-token";
}

internal sealed class TestPasswordHasher : IPasswordHasherService
{
    public string HashPassword(User user, string password) => $"HASHED::{password}";

    public PasswordVerificationResult VerifyHashedPassword(User user, string hashedPassword, string providedPassword)
        => hashedPassword == $"HASHED::{providedPassword}"
            ? PasswordVerificationResult.Success
            : PasswordVerificationResult.Failed;
}

internal sealed class TestCurrentUserAccessor(Guid? userId) : ICurrentUserAccessor
{
    public Guid? UserId { get; } = userId;
}
