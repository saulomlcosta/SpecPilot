namespace SpecPilot.Application.Abstractions.Auth;

public interface ICurrentUserAccessor
{
    Guid? UserId { get; }
}
