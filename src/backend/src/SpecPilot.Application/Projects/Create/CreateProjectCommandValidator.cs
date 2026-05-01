using FluentValidation;

namespace SpecPilot.Application.Projects.Create;

public class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
{
    public CreateProjectCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.InitialDescription)
            .NotEmpty();

        RuleFor(x => x.Goal)
            .NotEmpty();

        RuleFor(x => x.TargetAudience)
            .NotEmpty();
    }
}
