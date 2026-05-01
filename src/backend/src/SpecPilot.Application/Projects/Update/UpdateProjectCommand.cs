using MediatR;
using SpecPilot.Application.Projects.Common;
using SpecPilot.Domain.Common;

namespace SpecPilot.Application.Projects.Update;

public class UpdateProjectCommand : IRequest<Result<ProjectResponse>>
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string InitialDescription { get; init; } = string.Empty;
    public string Goal { get; init; } = string.Empty;
    public string TargetAudience { get; init; } = string.Empty;
}
