using FluentAssertions;
using SpecPilot.Application.Projects.Update;

namespace SpecPilot.UnitTests.Projects;

public class UpdateProjectCommandValidatorTests
{
    private readonly UpdateProjectCommandValidator _validator = new();

    [Fact]
    public void Should_fail_when_required_fields_are_invalid()
    {
        var command = new UpdateProjectCommand
        {
            Id = Guid.Empty,
            Name = string.Empty,
            InitialDescription = string.Empty,
            Goal = string.Empty,
            TargetAudience = string.Empty
        };

        var result = _validator.Validate(command);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.PropertyName == nameof(UpdateProjectCommand.Id));
        result.Errors.Should().Contain(x => x.PropertyName == nameof(UpdateProjectCommand.Name));
        result.Errors.Should().Contain(x => x.PropertyName == nameof(UpdateProjectCommand.InitialDescription));
        result.Errors.Should().Contain(x => x.PropertyName == nameof(UpdateProjectCommand.Goal));
        result.Errors.Should().Contain(x => x.PropertyName == nameof(UpdateProjectCommand.TargetAudience));
    }
}
