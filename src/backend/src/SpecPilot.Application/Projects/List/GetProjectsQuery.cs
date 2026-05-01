using MediatR;
using SpecPilot.Application.Projects.Common;
using SpecPilot.Domain.Common;

namespace SpecPilot.Application.Projects.List;

public record GetProjectsQuery : IRequest<Result<IReadOnlyList<ProjectResponse>>>;
