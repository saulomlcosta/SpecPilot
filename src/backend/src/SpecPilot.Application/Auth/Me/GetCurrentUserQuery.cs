using MediatR;
using SpecPilot.Application.Auth.Common;
using SpecPilot.Domain.Common;

namespace SpecPilot.Application.Auth.Me;

public class GetCurrentUserQuery : IRequest<Result<UserResponse>>
{
}
