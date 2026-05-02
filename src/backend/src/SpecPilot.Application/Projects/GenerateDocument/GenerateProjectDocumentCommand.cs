using MediatR;
using SpecPilot.Application.Projects.Common;
using SpecPilot.Domain.Common;

namespace SpecPilot.Application.Projects.GenerateDocument;

public record GenerateProjectDocumentCommand(Guid ProjectId) : IRequest<Result<ProjectDocumentResponse>>;
