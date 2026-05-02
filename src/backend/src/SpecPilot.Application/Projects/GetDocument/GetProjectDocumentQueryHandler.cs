using MediatR;
using Microsoft.EntityFrameworkCore;
using SpecPilot.Application.Abstractions.Auth;
using SpecPilot.Application.Abstractions.Persistence;
using SpecPilot.Application.Projects.Common;
using SpecPilot.Domain.Common;

namespace SpecPilot.Application.Projects.GetDocument;

public class GetProjectDocumentQueryHandler
    : IRequestHandler<GetProjectDocumentQuery, Result<ProjectDocumentResponse>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserAccessor _currentUserAccessor;

    public GetProjectDocumentQueryHandler(
        IApplicationDbContext context,
        ICurrentUserAccessor currentUserAccessor)
    {
        _context = context;
        _currentUserAccessor = currentUserAccessor;
    }

    public async Task<Result<ProjectDocumentResponse>> Handle(
        GetProjectDocumentQuery request,
        CancellationToken cancellationToken)
    {
        if (_currentUserAccessor.UserId is null)
        {
            return Result.Failure<ProjectDocumentResponse>(Error.Unauthorized(
                "projects.unauthenticated",
                "Usuario nao autenticado."));
        }

        var project = await _context.Projects
            .Include(x => x.ProjectDocument)
            .FirstOrDefaultAsync(
                x => x.Id == request.ProjectId && x.UserId == _currentUserAccessor.UserId.Value,
                cancellationToken);

        if (project is null)
        {
            return Result.Failure<ProjectDocumentResponse>(Error.NotFound(
                "projects.not_found",
                "Projeto nao encontrado."));
        }

        if (project.ProjectDocument is null)
        {
            return Result.Failure<ProjectDocumentResponse>(Error.NotFound(
                "projects.document_not_found",
                "Documento do projeto nao encontrado."));
        }

        return Result.Success(project.ProjectDocument.ToResponse());
    }
}
