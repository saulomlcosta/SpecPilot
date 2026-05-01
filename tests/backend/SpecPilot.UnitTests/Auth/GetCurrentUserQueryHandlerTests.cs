using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using SpecPilot.Application.Auth.Me;
using SpecPilot.Domain.Common;
using SpecPilot.Domain.Entities;
using SpecPilot.Infrastructure.Persistence;

namespace SpecPilot.UnitTests.Auth;

public class GetCurrentUserQueryHandlerTests
{
    [Fact]
    public async Task Should_return_authenticated_user()
    {
        await using var context = CreateContext();
        var user = new User
        {
            Name = "Saulo",
            Email = "saulo@example.com",
            PasswordHash = "HASHED::12345678"
        };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var handler = new GetCurrentUserQueryHandler(context, new TestCurrentUserAccessor(user.Id));

        var response = await handler.Handle(new GetCurrentUserQuery(), CancellationToken.None);

        response.IsSuccess.Should().BeTrue();
        response.Value!.Email.Should().Be("saulo@example.com");
        response.Value.Name.Should().Be("Saulo");
    }

    [Fact]
    public async Task Should_return_unauthorized_when_user_is_missing()
    {
        await using var context = CreateContext();
        var handler = new GetCurrentUserQueryHandler(context, new TestCurrentUserAccessor(Guid.NewGuid()));

        var response = await handler.Handle(new GetCurrentUserQuery(), CancellationToken.None);

        response.IsFailure.Should().BeTrue();
        response.Error!.Type.Should().Be(ErrorType.Unauthorized);
        response.Error.Code.Should().Be("auth.user_not_found");
    }

    private static SpecPilotDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<SpecPilotDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new SpecPilotDbContext(options);
    }
}
