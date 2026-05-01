using MediatR;
using Microsoft.EntityFrameworkCore;
using SpecPilot.Application.Abstractions.Auth;
using SpecPilot.Application.Abstractions.Persistence;
using SpecPilot.Domain.Common;

namespace SpecPilot.Application.Projects.Delete;

public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand, Result>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserAccessor _currentUserAccessor;

    public DeleteProjectCommandHandler(IApplicationDbContext context, ICurrentUserAccessor currentUserAccessor)
    {
        _context = context;
        _currentUserAccessor = currentUserAccessor;
    }

    public async Task<Result> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
    {
        if (_currentUserAccessor.UserId is null)
        {
            return Result.Failure(Error.Unauthorized(
                "projects.unauthenticated",
                "Usuario nao autenticado."));
        }

        var project = await _context.Projects
            .FirstOrDefaultAsync(
                x => x.Id == request.Id && x.UserId == _currentUserAccessor.UserId.Value,
                cancellationToken);

        if (project is null)
        {
            return Result.Failure(Error.NotFound(
                "projects.not_found",
                "Projeto nao encontrado."));
        }

        _context.Projects.Remove(project);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
