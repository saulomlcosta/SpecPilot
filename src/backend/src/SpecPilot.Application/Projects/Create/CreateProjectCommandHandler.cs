using MediatR;
using SpecPilot.Application.Abstractions.Auth;
using SpecPilot.Application.Abstractions.Persistence;
using SpecPilot.Application.Projects.Common;
using SpecPilot.Domain.Common;
using SpecPilot.Domain.Entities;
using SpecPilot.Domain.Enums;

namespace SpecPilot.Application.Projects.Create;

public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, Result<ProjectResponse>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserAccessor _currentUserAccessor;

    public CreateProjectCommandHandler(IApplicationDbContext context, ICurrentUserAccessor currentUserAccessor)
    {
        _context = context;
        _currentUserAccessor = currentUserAccessor;
    }

    public async Task<Result<ProjectResponse>> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        if (_currentUserAccessor.UserId is null)
        {
            return Result.Failure<ProjectResponse>(Error.Unauthorized(
                "projects.unauthenticated",
                "Usuario nao autenticado."));
        }

        var project = new Project
        {
            UserId = _currentUserAccessor.UserId.Value,
            Name = request.Name.Trim(),
            InitialDescription = request.InitialDescription.Trim(),
            Goal = request.Goal.Trim(),
            TargetAudience = request.TargetAudience.Trim(),
            Status = ProjectStatus.Draft
        };

        _context.Projects.Add(project);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success(project.ToResponse());
    }
}
