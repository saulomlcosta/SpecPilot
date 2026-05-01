using FluentAssertions;
using SpecPilot.Application.Projects.Create;

namespace SpecPilot.UnitTests.Projects;

public class CreateProjectCommandValidatorTests
{
    private readonly CreateProjectCommandValidator _validator = new();

    [Fact]
    public void Should_fail_when_required_fields_are_empty()
    {
        var command = new CreateProjectCommand
        {
            Name = string.Empty,
            InitialDescription = string.Empty,
            Goal = string.Empty,
            TargetAudience = string.Empty
        };

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.PropertyName == nameof(CreateProjectCommand.Name));
        result.Errors.Should().Contain(x => x.PropertyName == nameof(CreateProjectCommand.InitialDescription));
        result.Errors.Should().Contain(x => x.PropertyName == nameof(CreateProjectCommand.Goal));
        result.Errors.Should().Contain(x => x.PropertyName == nameof(CreateProjectCommand.TargetAudience));
    }
}
