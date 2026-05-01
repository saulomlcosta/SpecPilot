using MediatR;
using SpecPilot.Application.Projects.Common;
using SpecPilot.Domain.Common;

namespace SpecPilot.Application.Projects.GetById;

public record GetProjectByIdQuery(Guid Id) : IRequest<Result<ProjectResponse>>;
