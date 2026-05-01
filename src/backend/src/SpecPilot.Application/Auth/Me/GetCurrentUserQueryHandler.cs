using MediatR;
using Microsoft.EntityFrameworkCore;
using SpecPilot.Application.Abstractions.Auth;
using SpecPilot.Application.Abstractions.Persistence;
using SpecPilot.Application.Auth.Common;
using SpecPilot.Domain.Common;

namespace SpecPilot.Application.Auth.Me;

public class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, Result<UserResponse>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserAccessor _currentUserAccessor;

    public GetCurrentUserQueryHandler(IApplicationDbContext context, ICurrentUserAccessor currentUserAccessor)
    {
        _context = context;
        _currentUserAccessor = currentUserAccessor;
    }

    public async Task<Result<UserResponse>> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        if (_currentUserAccessor.UserId is null)
        {
            return Result.Failure<UserResponse>(Error.Unauthorized(
                "auth.unauthenticated",
                "Usuario nao autenticado."));
        }

        var user = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == _currentUserAccessor.UserId.Value, cancellationToken);

        if (user is null)
        {
            return Result.Failure<UserResponse>(Error.Unauthorized(
                "auth.user_not_found",
                "Usuario nao autenticado."));
        }

        return Result.Success(new UserResponse
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email
        });
    }
}
