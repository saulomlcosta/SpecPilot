using MediatR;
using SpecPilot.Application.Projects.Common;
using SpecPilot.Domain.Common;

namespace SpecPilot.Application.Projects.GetDocument;

public record GetProjectDocumentQuery(Guid ProjectId) : IRequest<Result<ProjectDocumentResponse>>;
