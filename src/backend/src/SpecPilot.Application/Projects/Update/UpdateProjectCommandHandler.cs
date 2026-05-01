using MediatR;
using Microsoft.EntityFrameworkCore;
using SpecPilot.Application.Abstractions.Auth;
using SpecPilot.Application.Abstractions.Persistence;
using SpecPilot.Application.Projects.Common;
using SpecPilot.Domain.Common;
using SpecPilot.Domain.Enums;

namespace SpecPilot.Application.Projects.Update;

public class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand, Result<ProjectResponse>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserAccessor _currentUserAccessor;

    public UpdateProjectCommandHandler(IApplicationDbContext context, ICurrentUserAccessor currentUserAccessor)
    {
        _context = context;
        _currentUserAccessor = currentUserAccessor;
    }

    public async Task<Result<ProjectResponse>> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        if (_currentUserAccessor.UserId is null)
        {
            return Result.Failure<ProjectResponse>(Error.Unauthorized(
                "projects.unauthenticated",
                "Usuario nao autenticado."));
        }

        var project = await _context.Projects
            .FirstOrDefaultAsync(
                x => x.Id == request.Id && x.UserId == _currentUserAccessor.UserId.Value,
                cancellationToken);

        if (project is null)
        {
            return Result.Failure<ProjectResponse>(Error.NotFound(
                "projects.not_found",
                "Projeto nao encontrado."));
        }

        project.Name = request.Name.Trim();
        project.InitialDescription = request.InitialDescription.Trim();
        project.Goal = request.Goal.Trim();
        project.TargetAudience = request.TargetAudience.Trim();
        project.Status = Enum.Parse<ProjectStatus>(request.Status, true);
        project.UpdatedAtUtc = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success(project.ToResponse());
    }
}
