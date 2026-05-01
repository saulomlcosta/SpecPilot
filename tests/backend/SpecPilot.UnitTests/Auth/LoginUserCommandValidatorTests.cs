using FluentAssertions;
using SpecPilot.Application.Auth.Login;

namespace SpecPilot.UnitTests.Auth;

public class LoginUserCommandValidatorTests
{
    private readonly LoginUserCommandValidator _validator = new();

    [Fact]
    public void Should_fail_when_email_is_empty()
    {
        var command = new LoginUserCommand
        {
            Email = string.Empty,
            Password = "12345678"
        };

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.PropertyName == nameof(LoginUserCommand.Email));
    }
}
