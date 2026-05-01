using FluentValidation;
using SpecPilot.Domain.Enums;

namespace SpecPilot.Application.Projects.Update;

public class UpdateProjectCommandValidator : AbstractValidator<UpdateProjectCommand>
{
    public UpdateProjectCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.InitialDescription)
            .NotEmpty();

        RuleFor(x => x.Goal)
            .NotEmpty();

        RuleFor(x => x.TargetAudience)
            .NotEmpty();

        RuleFor(x => x.Status)
            .NotEmpty()
            .Must(status => Enum.TryParse<ProjectStatus>(status, true, out _))
            .WithMessage("O status informado e invalido.");
    }
}
