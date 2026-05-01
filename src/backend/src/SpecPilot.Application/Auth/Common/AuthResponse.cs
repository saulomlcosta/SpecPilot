namespace SpecPilot.Application.Auth.Common;

public class AuthResponse
{
    public string Token { get; init; } = string.Empty;
    public UserResponse User { get; init; } = new();
}
