using MediatR;
using Microsoft.EntityFrameworkCore;
using SpecPilot.Application.Abstractions.Auth;
using SpecPilot.Application.Abstractions.Persistence;
using SpecPilot.Application.Projects.Common;
using SpecPilot.Domain.Common;

namespace SpecPilot.Application.Projects.GetById;

public class GetProjectByIdQueryHandler : IRequestHandler<GetProjectByIdQuery, Result<ProjectResponse>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserAccessor _currentUserAccessor;

    public GetProjectByIdQueryHandler(IApplicationDbContext context, ICurrentUserAccessor currentUserAccessor)
    {
        _context = context;
        _currentUserAccessor = currentUserAccessor;
    }

    public async Task<Result<ProjectResponse>> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
    {
        if (_currentUserAccessor.UserId is null)
        {
            return Result.Failure<ProjectResponse>(Error.Unauthorized(
                "projects.unauthenticated",
                "Usuario nao autenticado."));
        }

        var project = await _context.Projects
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.Id == request.Id && x.UserId == _currentUserAccessor.UserId.Value,
                cancellationToken);

        if (project is null)
        {
            return Result.Failure<ProjectResponse>(Error.NotFound(
                "projects.not_found",
                "Projeto nao encontrado."));
        }

        return Result.Success(project.ToResponse());
    }
}
