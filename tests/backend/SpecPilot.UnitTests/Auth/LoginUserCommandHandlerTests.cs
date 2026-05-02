using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SpecPilot.Application.Auth.Login;
using SpecPilot.Domain.Common;
using SpecPilot.Domain.Entities;
using SpecPilot.Infrastructure.Persistence;

namespace SpecPilot.UnitTests.Auth;

public class LoginUserCommandHandlerTests
{
    [Fact]
    public async Task Should_return_token_for_valid_credentials()
    {
        await using var context = CreateContext();
        context.Users.Add(new User
        {
            Name = "Saulo",
            Email = "saulo@example.com",
            PasswordHash = "HASHED::12345678"
        });
        await context.SaveChangesAsync();

        var handler = new LoginUserCommandHandler(
            context,
            new TestPasswordHasher(),
            new TestJwtTokenGenerator(),
            TestLogger<LoginUserCommandHandler>.Instance);

        var response = await handler.Handle(new LoginUserCommand
        {
            Email = "saulo@example.com",
            Password = "12345678"
        }, CancellationToken.None);

        response.IsSuccess.Should().BeTrue();
        response.Value!.Token.Should().Be("fake-jwt-token");
        response.Value.User.Name.Should().Be("Saulo");
    }

    [Fact]
    public async Task Should_return_unauthorized_for_invalid_credentials()
    {
        await using var context = CreateContext();
        var handler = new LoginUserCommandHandler(
            context,
            new TestPasswordHasher(),
            new TestJwtTokenGenerator(),
            TestLogger<LoginUserCommandHandler>.Instance);

        var response = await handler.Handle(new LoginUserCommand
        {
            Email = "saulo@example.com",
            Password = "senha-invalida"
        }, CancellationToken.None);

        response.IsFailure.Should().BeTrue();
        response.Error!.Type.Should().Be(ErrorType.Unauthorized);
        response.Error.Code.Should().Be("auth.invalid_credentials");
    }

    private static SpecPilotDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<SpecPilotDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new SpecPilotDbContext(options);
    }
}
