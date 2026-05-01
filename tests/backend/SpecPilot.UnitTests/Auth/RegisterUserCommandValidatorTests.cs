using FluentAssertions;
using SpecPilot.Application.Auth.Register;

namespace SpecPilot.UnitTests.Auth;

public class RegisterUserCommandValidatorTests
{
    private readonly RegisterUserCommandValidator _validator = new();

    [Fact]
    public void Should_fail_when_email_is_invalid()
    {
        var command = new RegisterUserCommand
        {
            Name = "Saulo",
            Email = "email-invalido",
            Password = "12345678"
        };

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.PropertyName == nameof(RegisterUserCommand.Email));
    }

    [Fact]
    public void Should_fail_when_password_is_too_short()
    {
        var command = new RegisterUserCommand
        {
            Name = "Saulo",
            Email = "saulo@example.com",
            Password = "123"
        };

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.PropertyName == nameof(RegisterUserCommand.Password));
    }
}
