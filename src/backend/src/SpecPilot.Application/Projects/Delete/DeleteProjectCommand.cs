using MediatR;
using SpecPilot.Domain.Common;

namespace SpecPilot.Application.Projects.Delete;

public record DeleteProjectCommand(Guid Id) : IRequest<Result>;
