using MediatR;
using SpecPilot.Application.Projects.Common;
using SpecPilot.Domain.Common;

namespace SpecPilot.Application.Projects.Create;

public class CreateProjectCommand : IRequest<Result<ProjectResponse>>
{
    public string Name { get; init; } = string.Empty;
    public string InitialDescription { get; init; } = string.Empty;
    public string Goal { get; init; } = string.Empty;
    public string TargetAudience { get; init; } = string.Empty;
}
