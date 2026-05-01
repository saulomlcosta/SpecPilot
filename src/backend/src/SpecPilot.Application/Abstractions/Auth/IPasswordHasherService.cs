using Microsoft.AspNetCore.Identity;
using SpecPilot.Domain.Entities;

namespace SpecPilot.Application.Abstractions.Auth;

public interface IPasswordHasherService
{
    string HashPassword(User user, string password);
    PasswordVerificationResult VerifyHashedPassword(User user, string hashedPassword, string providedPassword);
}
