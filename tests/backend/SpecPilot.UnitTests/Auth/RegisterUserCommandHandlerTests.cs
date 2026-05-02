using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SpecPilot.Application.Auth.Register;
using SpecPilot.Domain.Common;
using SpecPilot.Domain.Entities;
using SpecPilot.Infrastructure.Persistence;

namespace SpecPilot.UnitTests.Auth;

public class RegisterUserCommandHandlerTests
{
    [Fact]
    public async Task Should_create_user_and_return_token()
    {
        await using var context = CreateContext();
        var handler = new RegisterUserCommandHandler(
            context,
            new TestPasswordHasher(),
            new TestJwtTokenGenerator(),
            TestLogger<RegisterUserCommandHandler>.Instance);

        var command = new RegisterUserCommand
        {
            Name = "Saulo",
            Email = "saulo@example.com",
            Password = "12345678"
        };

        var response = await handler.Handle(command, CancellationToken.None);

        response.IsSuccess.Should().BeTrue();
        response.Value!.Token.Should().Be("fake-jwt-token");
        response.Value.User.Email.Should().Be("saulo@example.com");
        context.Users.Should().ContainSingle();
        context.Users.Single().PasswordHash.Should().Be("HASHED::12345678");
    }

    [Fact]
    public async Task Should_return_conflict_when_email_is_duplicated()
    {
        await using var context = CreateContext();
        context.Users.Add(new User
        {
            Name = "Saulo",
            Email = "saulo@example.com",
            PasswordHash = "HASHED::12345678"
        });
        await context.SaveChangesAsync();

        var handler = new RegisterUserCommandHandler(
            context,
            new TestPasswordHasher(),
            new TestJwtTokenGenerator(),
            TestLogger<RegisterUserCommandHandler>.Instance);

        var command = new RegisterUserCommand
        {
            Name = "Outro",
            Email = "saulo@example.com",
            Password = "12345678"
        };

        var response = await handler.Handle(command, CancellationToken.None);

        response.IsFailure.Should().BeTrue();
        response.Error!.Type.Should().Be(ErrorType.Conflict);
        response.Error.Code.Should().Be("auth.email_already_registered");
    }

    private static SpecPilotDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<SpecPilotDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new SpecPilotDbContext(options);
    }
}
