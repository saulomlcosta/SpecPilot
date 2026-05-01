using MediatR;
using Microsoft.EntityFrameworkCore;
using SpecPilot.Application.Abstractions.Auth;
using SpecPilot.Application.Abstractions.Persistence;
using SpecPilot.Application.Projects.Common;
using SpecPilot.Domain.Common;

namespace SpecPilot.Application.Projects.List;

public class GetProjectsQueryHandler : IRequestHandler<GetProjectsQuery, Result<IReadOnlyList<ProjectResponse>>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserAccessor _currentUserAccessor;

    public GetProjectsQueryHandler(IApplicationDbContext context, ICurrentUserAccessor currentUserAccessor)
    {
        _context = context;
        _currentUserAccessor = currentUserAccessor;
    }

    public async Task<Result<IReadOnlyList<ProjectResponse>>> Handle(GetProjectsQuery request, CancellationToken cancellationToken)
    {
        if (_currentUserAccessor.UserId is null)
        {
            return Result.Failure<IReadOnlyList<ProjectResponse>>(Error.Unauthorized(
                "projects.unauthenticated",
                "Usuario nao autenticado."));
        }

        var projects = await _context.Projects
            .AsNoTracking()
            .Where(x => x.UserId == _currentUserAccessor.UserId.Value)
            .OrderByDescending(x => x.CreatedAtUtc)
            .ToListAsync(cancellationToken);

        return Result.Success<IReadOnlyList<ProjectResponse>>(projects.Select(x => x.ToResponse()).ToList());
    }
}
